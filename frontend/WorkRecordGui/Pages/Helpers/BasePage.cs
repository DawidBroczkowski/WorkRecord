using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages.Helpers
{
    public class BasePage : ContentPage
    {
        protected readonly IPageModelFactory _pageModelFactory;
        protected readonly INavigationService _navigationService;
        private Dictionary<Type, BaseViewModel> _viewModelCache = new();

        public BasePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory)
        {
            _pageModelFactory = pageModelFactory;
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await((BaseViewModel)this.BindingContext).InitializeAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var viewModel = _navigationService.GetMappedViewModel(GetType());
            _pageModelFactory.ClearViewModel(viewModel);
        }
    }
}
