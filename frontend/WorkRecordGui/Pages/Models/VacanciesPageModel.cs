using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.Vacancy;
using WorkRecordGui.Shared.Dtos.Vacancy;
using WorkRecordGui.Shared.Dtos.WeekPlan;

namespace WorkRecordGui.Pages.Models
{
    public class PositionGroup : ObservableCollection<GetVacancyDto>
    {
        public string Position { get; private set; }

        public PositionGroup(string position, IEnumerable<GetVacancyDto> vacancies) : base(vacancies)
        {
            Position = position;
        }
    }

    public class VacanciesPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IVacancyService _vacancyService;
        private INavigationService _navigationService;
        private IPlanManagerService _planManagerService;
        private IWeekPlanService _weekPlanService;
        private int _weekPlanId;
        public int WeekPlanId
        {
            get => _weekPlanId;
            set
            {
                _weekPlanId = value;
                OnPropertyChanged();
            }
        }
        private int _selectedPlanId;
        public int SelectedPlanId
        {
            get => _selectedPlanId;
            set
            {
                if (_selectedPlanId == value) return;
                _selectedPlanId = value;
                loadVacancies(SelectedPlanId); // Load vacancies for the selected week plan
                OnPropertyChanged();
            }
        }
        private string _planName;
        public string PlanName
        {
            get => _planName;
            set
            {
                _planName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionGroup> Positions { get; set; } = new();
        public ObservableCollection<int> WeekPlans { get; set; } = new();

        private List<GetWeekPlanDto> _weekPlans;

        public ICommand VacancyTappedCommand { get; }
        public ICommand AddVacancyTappedCommand { get; }
        public ICommand SwitchPlanTappedCommand { get; }
        public ICommand AddPlanTappedCommand { get; }

        public VacanciesPageModel(int weekPlanId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _planManagerService = _serviceProvider.GetRequiredService<IPlanManagerService>();
            _weekPlanService = _serviceProvider.GetRequiredService<IWeekPlanService>();

            VacancyTappedCommand = new Command<int>(navigateToVacancy);
            AddVacancyTappedCommand = new Command(() => _navigationService.NavigateToAsync(typeof(AddVacancyPageModel)));
            SwitchPlanTappedCommand = new Command(switchActivePlanAsync);
            AddPlanTappedCommand = new Command(async () => await addPlanAsync());
        }

        private async Task loadVacancies(int weekPlanId)
        {
            Positions.Clear();

            // Load vacancies from the database
            var vacancies = await _vacancyService.GetVacanciesByWeekPlanIdAsync(weekPlanId, _cts.Token);

            var groupedVacancies = vacancies
                .OrderBy(v => v.OccurrenceDay)
                .GroupBy(v => v.Position.ToString())
                .Select(g => new PositionGroup(g.Key!, g))
                .ToList();

            foreach (var group in groupedVacancies)
            {
                Positions.Add(group);
            }
        }

        private async void navigateToVacancy(int vacancyId)
        {
            await _navigationService.NavigateToAsync(typeof(VacancyPageModel), vacancyId);
        }

        private async void switchActivePlanAsync()
        {
            try
            {
                await _planManagerService.ChangeCurrentPlanAsync(SelectedPlanId, _cts.Token);
                await loadVacancies(SelectedPlanId);
                WeekPlanId = SelectedPlanId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task addPlanAsync()
        {
            try
            {
                await _weekPlanService.AddWeekPlanAsync(PlanName, _cts.Token);
                await loadWeekPlansAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task loadWeekPlansAsync()
        {
            _weekPlans = await _weekPlanService.GetWeekPlansAsync(_cts.Token);
            WeekPlans = new ObservableCollection<int>(_weekPlans.Select(w => w.Id));
            OnPropertyChanged(nameof(WeekPlans));
        }

        public override async Task InitializeAsync()
        {
            _selectedPlanId = await _planManagerService.GetCurrentPlanIdAsync();
            _weekPlanId = _selectedPlanId;
            await loadVacancies(SelectedPlanId);
            await loadWeekPlansAsync();
            OnPropertyChanged(nameof(WeekPlanId));
            OnPropertyChanged(nameof(SelectedPlanId));
        }
    }
}
