using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
  public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupService;
        private IMeetingLookupDataService _meetingLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupService, IMeetingLookupDataService meetingLookupService, IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator
                .GetEvent<AfterDetailSavedEvent>()
                .Subscribe(
                    obj =>
                        {
                            switch (obj.ViewModelName)
                            {
                                case nameof(FriendDetailViewModel):
                                    AfterDetailSaved(Friends, obj);
                                    break;
                                case nameof(MeetingDetailViewModel):
                                    AfterDetailSaved(Meetings, obj);
                                    break;
                            }
                        });
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName,
                    _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));
            }

            lookup = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, 
                    item.DisplayMember,
                    nameof(MeetingDetailViewModel),
                    _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            ObservableCollection<NavigationItemViewModel> navigationItemViewModels = null;
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    navigationItemViewModels = Friends;
                    break;
                case nameof(MeetingDetailViewModel):
                    navigationItemViewModels = Meetings;
                    break;
            }

            var navigationItemViewModel = navigationItemViewModels.SingleOrDefault(f => f.Id == args.Id);
            if (navigationItemViewModel != null)
            {
                navigationItemViewModels.Remove(navigationItemViewModel);
            }
        }
    }
}
