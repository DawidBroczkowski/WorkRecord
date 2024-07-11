using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using WorkRecordGui.Model;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages.Models
{
    public class ReportPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IReportService _reportService;
        private IFileService _fileService;
        private IFolderPicker _folderPicker;

        private DateOnly _selectedDate = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        private string directory = "";
        public string Directory
        {
            get => directory;
            set
            {
                directory = value;
                OnPropertyChanged();
            }
        }

        private string _fileName = "";
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetReportCommand => new Command(async () => await getReportAsync());
        public ICommand PickDirectoryCommand => new Command(async () => await pickDirectoryAsync());

        public ReportPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _reportService = serviceProvider.GetRequiredService<IReportService>();
            _fileService = serviceProvider.GetRequiredService<IFileService>();
            _folderPicker = serviceProvider.GetRequiredService<IFolderPicker>();
        }

        private async Task getReportAsync()
        {
            var report = await _reportService.GetReportAsync(SelectedDate, _cts.Token);
            if (System.IO.Directory.Exists(Directory) is false)
            {
                return;
            }
            var fileName = $"{FileName}.pdf";
            var filePath = Path.Combine(Directory, fileName);
            await _fileService.SaveToFileAsync(report, filePath, _cts.Token);
        }

        private async Task pickDirectoryAsync()
        {
            try
            {
                var result = await _folderPicker.PickAsync(_cts.Token);
                if (result.IsSuccessful)
                {
                    Directory = result.Folder.Path;
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
