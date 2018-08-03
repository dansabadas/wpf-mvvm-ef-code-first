using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private readonly Func<IMeetingDetailViewModel> _meetingDetailViewModelCreator;
        private IDetailViewModel _detailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            Func<IMeetingDetailViewModel> meetingDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _meetingDetailViewModelCreator = meetingDetailViewModelCreator;

            NavigationViewModel = navigationViewModel;
            CreateNewDetailCommand = new DelegateCommand<Type>(viewModelType => OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name }));

            eventAggregator
                .GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);
        }

        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }

        public ICommand CreateNewDetailCommand { get; }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _friendDetailViewModelCreator();
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailViewModel = _meetingDetailViewModelCreator();
                    break;
                default:
                    throw new Exception("err");
            }

            await DetailViewModel.LoadAsync(args.Id);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs arg)
        {
            DetailViewModel = null;
        }
    }
}
