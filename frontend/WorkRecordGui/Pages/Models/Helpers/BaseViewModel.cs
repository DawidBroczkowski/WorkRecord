using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkRecordGui.Models;

namespace WorkRecordGui.Pages.Models.Helpers
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected CancellationTokenSource _cts = new CancellationTokenSource();

        private Session _session;
        public Session Session 
        {   get
            {
                return _session;
            }
        }

        public BaseViewModel(IServiceProvider serviceProvider)
        {
            _session = serviceProvider.GetRequiredService<Session>();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
