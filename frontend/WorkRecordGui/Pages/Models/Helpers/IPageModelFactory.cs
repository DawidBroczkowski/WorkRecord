namespace WorkRecordGui.Pages.Models.Helpers
{
    public interface IPageModelFactory
    {
        BaseViewModel CreateViewModel(Type viewModelType, params object[] parameters);
        public void ClearViewModel(Type viewModelType);
    }
}