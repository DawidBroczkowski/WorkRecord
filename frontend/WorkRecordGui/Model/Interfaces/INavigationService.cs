using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Model.Interfaces
{
    public interface INavigationService
    {
        int GetStackSize();
        Task GoBackAsync();
        Task NavigateToAsync(Type pageType, params object[] parameters);
        Type GetMappedViewModel(Type viewType);
    }
}