using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.ChartEntry;
using System.Drawing;
using System.Reflection;
using WorkRecord.Shared.Dtos.LeaveEntry;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure.PdfGeneration
{
    public class MonthlyLeaveSummaryPage : IDocument
    {
        private List<GetLeaveEntryDto> _leaveEntries = new();
        private string _mostLeavesMonth = string.Empty;
        private LeaveType _mostLeavesType;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Header().Text("Monthly Leave Summary").FontSize(24).FontColor(Colors.Grey.Darken4).AlignLeft();
                page.Footer().Text($"Generated on: {DateTime.Now.ToString("dd/MM/yyyy")}").FontSize(16).Italic().AlignLeft();
                page.Content().Padding(20).Column(column =>
                {
                    column.Item().Text("Year: 2024").FontSize(20).Bold().AlignLeft();
                    column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    column.Item().Element(ComposeTable);
                    column.Item().Text("Item 1 - amount of leaves active in a given month, divided by leave type").FontSize(10).AlignLeft();
                    column.Item().Element(ComposeCharts);
                    column.Item().Text("Item 2 - chart of leaves active in a given month, divided by leave type").FontSize(10).AlignLeft();
                    column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    column.Item().Text($"The month with the most leaves: {_mostLeavesMonth}").FontSize(12).AlignLeft();
                    column.Item().Text($"The most common leave type: {_mostLeavesType}").FontSize(12).AlignLeft();
                });
            });
        }

        void ComposeCharts(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Image(GenerateBarChart());
            });
        }

        Stream GenerateBarChart()
        {
            var data = GetMonthlyLeaveData();

            // Determine the maximum value of any leave type in every month
            var maxPaidlLeaves = data.Max(d => d.PaidLeaves);
            var maxSickLeaves = data.Max(d => d.SickLeaves);
            var maxMaternityLeaves = data.Max(d => d.MaternityLeaves);
            var maxOtherLeaves = data.Max(d => d.Other);
            var maxTotalLeaves = new[] { maxPaidlLeaves, maxSickLeaves, maxMaternityLeaves, maxOtherLeaves }.Max();

            // Create entries for the bar chart
            var entries = data.SelectMany(d => new[]
            {
                new Entry(d.PaidLeaves)
                {
                    Label = $"{d.Month} Paid",
                    ValueLabel = d.PaidLeaves.ToString(),
                    Color = SKColor.Parse("#3498db")  // Color for Paid Leaves
                },
                new Entry(d.SickLeaves)
                {
                    Label = $"{d.Month} Sick",
                    ValueLabel = d.SickLeaves.ToString(),
                    Color = SKColor.Parse("#e74c3c")  // Color for Sick Leaves
                },
                new Entry(d.MaternityLeaves)
                {
                    Label = $"{d.Month} Maternity",
                    ValueLabel = d.MaternityLeaves.ToString(),
                    Color = SKColor.Parse("#2ecc71")  // Color for Maternity Leaves
                },
                new Entry(d.Other)
                {
                    Label = $"{d.Month} Other",
                    ValueLabel = d.Other.ToString(),
                    Color = SKColor.Parse("#f1c40f")  // Color for Other Leaves
                }
            }).ToArray();

            var chart = new BarChart
            {
                Entries = entries,
                IsAnimated = false,
                MaxValue = maxTotalLeaves,
                Margin = 2
            };

            // Create a bitmap for the chart
            var bmp = new SKBitmap(800, 800); // Adjust the size as needed
            var canvas = new SKCanvas(bmp);
            canvas.Clear(SKColor.Parse("#ffffff")); // Set background to white
            chart.DrawContent(canvas, 800, 600);

            // Draw the legend manually
            DrawLegend(canvas);

            var memoryStream = new MemoryStream();
            using (var image = SKImage.FromPixels(bmp.PeekPixels()))
            using (var encodedImage = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                encodedImage.SaveTo(memoryStream);
            }

            // Reset the position of the stream to the beginning before returning
            memoryStream.Position = 0;

            return memoryStream;
        }

        void DrawLegend(SKCanvas canvas)
        {
            var legendX = 0; // X position of the legend
            var legendY = 600;  // Y position of the legend
            var legendSpacing = 20; // Spacing between legend items

            var colors = new[]
            {
                SKColor.Parse("#3498db"),
                SKColor.Parse("#e74c3c"),
                SKColor.Parse("#2ecc71"),
                SKColor.Parse("#f1c40f")
            };

            var labels = new[]
            {
                "Paid Leaves",
                "Sick Leaves",
                "Maternity Leaves",
                "Other"
            };

            for (int i = 0; i < colors.Length; i++)
            {
                // Draw color box
                var paint = new SKPaint
                {
                    Color = colors[i],
                    Style = SKPaintStyle.Fill
                };
                canvas.DrawRect(legendX, legendY + i * legendSpacing, 20, 20, paint);

                // Draw label
                var textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 16,
                    IsAntialias = true
                };
                canvas.DrawText(labels[i], legendX + 30, legendY + 15 + i * legendSpacing, textPaint);
            }
        }

        void ComposeTable(IContainer container)
        {
            container
            .Padding(10)
            .AlignCenter()
            .MinimalBox()
            .Border(1)
            .Table(table =>
            {
                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                {
                    return container
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1)
                        .Background(backgroundColor)
                        .PaddingVertical(5)
                        .PaddingHorizontal(10)
                        .AlignCenter()
                        .AlignMiddle();
                }

                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Month");
                    header.Cell().Element(CellStyle).Text("Total Leaves");
                    header.Cell().Element(CellStyle).Text("Paid Leaves");
                    header.Cell().Element(CellStyle).Text("Sick Leaves");
                    header.Cell().Element(CellStyle).Text("Maternity Leaves");
                    header.Cell().Element(CellStyle).Text("Other");
                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });

                foreach (var monthData in GetMonthlyLeaveData())
                {
                    table.Cell().Element(CellStyle).Text(monthData.Month);
                    table.Cell().Element(CellStyle).Text(monthData.TotalLeaves.ToString());
                    table.Cell().Element(CellStyle).Text(monthData.PaidLeaves.ToString());
                    table.Cell().Element(CellStyle).Text(monthData.SickLeaves.ToString());
                    table.Cell().Element(CellStyle).Text(monthData.MaternityLeaves.ToString());
                    table.Cell().Element(CellStyle).Text(monthData.Other.ToString());
                }

                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White);
            });
        }

        public void LoadData(List<GetLeaveEntryDto> leaveEntries)
        {
            _leaveEntries = leaveEntries;
        }

        List<MonthlyLeaveData> GetMonthlyLeaveData()
        {
            List<MonthlyLeaveData> data = new();

            foreach (var month in Enumerable.Range(1, 12))
            {
                var monthName = new DateTime(2024, month, 1).ToString("MMMM");
                var monthData = new MonthlyLeaveData
                {
                    Month = monthName,
                    TotalLeaves = 0,
                    PaidLeaves = 0,
                    SickLeaves = 0,
                    MaternityLeaves = 0,
                    Other = 0
                };

                foreach (var leave in _leaveEntries)
                {
                    if (leave.StartDate.Month == month)
                    {
                        monthData.TotalLeaves++;
                        switch (leave.LeaveType)
                        {
                            case LeaveType.paid:
                                monthData.PaidLeaves++;
                                break;
                            case LeaveType.sick:
                                monthData.SickLeaves++;
                                break;
                            case LeaveType.maternity:
                                monthData.MaternityLeaves++;
                                break;
                            default:
                                monthData.Other++;
                                break;
                        }
                    }
                }

                data.Add(monthData);
            }

            _mostLeavesMonth = data.OrderByDescending(d => d.TotalLeaves).First().Month;
            _mostLeavesType = _leaveEntries
                .GroupBy(l => l.LeaveType)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
            return data;
        }

        class MonthlyLeaveData
        {
            public string Month { get; set; }
            public int TotalLeaves { get; set; }
            public int PaidLeaves { get; set; }
            public int SickLeaves { get; set; }
            public int MaternityLeaves { get; set; }
            public int Other { get; set; }
        }
    }
}
