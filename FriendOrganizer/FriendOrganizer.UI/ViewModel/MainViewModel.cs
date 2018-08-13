using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.View.Services;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly object _syncLock = new object();
        private IMessageDialogService _messageDialogService;
        private readonly IIndex<string, IDetailViewModel> _detailViewModelCreator;
        private IDetailViewModel _selectedDetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _detailViewModelCreator = detailViewModelCreator;
            _messageDialogService = messageDialogService;

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            NavigationViewModel = navigationViewModel;

            int nextNewItemId = 0;
            CreateNewDetailCommand = new DelegateCommand<Type>(viewModelType => OnOpenDetailView(new OpenDetailViewEventArgs
            {
                Id = nextNewItemId--,
                ViewModelName = viewModelType.Name
            }));

            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);

            eventAggregator
                .GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(args => RemoveDetailViewModel(args.Id, args.ViewModelName));
            eventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Subscribe(args => RemoveDetailViewModel(args.Id, args.ViewModelName));
        }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get => _selectedDetailViewModel;
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }

        public ICommand CreateNewDetailCommand { get; }

        public ICommand OpenSingleDetailViewCommand { get; }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            var detailViewModel =
                DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch
                {
                    _messageDialogService.ShowInfoDialog("Could not load the entity, " +
                                                         "maybe it was deleted in the meantime by another user. " +
                                                         "The navigation is refreshed for you.");
                    await NavigationViewModel.LoadAsync();
                    return;
                }
                lock(_syncLock)
                {
                    if (!DetailViewModels.Any(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName))
                    {
                        DetailViewModels.Add(detailViewModel);
                    }
                }
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(
                new OpenDetailViewEventArgs
                {
                    Id = -1,
                    ViewModelName = viewModelType.Name
                });
        }
    }
}
