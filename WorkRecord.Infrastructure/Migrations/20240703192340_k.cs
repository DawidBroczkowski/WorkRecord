using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkRecord.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class k : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartEntries_Vacancies_VacancyId",
                table: "ChartEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartEntries_Vacancies_VacancyId",
                table: "ChartEntries",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartEntries_Vacancies_VacancyId",
                table: "ChartEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartEntries_Vacancies_VacancyId",
                table: "ChartEntries",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
