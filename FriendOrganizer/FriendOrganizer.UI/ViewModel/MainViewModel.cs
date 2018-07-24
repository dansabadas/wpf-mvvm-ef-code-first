using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IMessageDialogService _messageDialogService;
    private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
    private IFriendDetailViewModel _friendDetailViewModel;

    public MainViewModel(INavigationViewModel navigationViewModel, 
      Func<IFriendDetailViewModel> friendDetailViewModelCreator, 
      IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService)
    {
      _eventAggregator = eventAggregator;
      _messageDialogService = messageDialogService;
      _friendDetailViewModelCreator = friendDetailViewModelCreator;
      NavigationViewModel = navigationViewModel;

      _eventAggregator
            .GetEvent<OpenFriendDetailViewEvent>()
            .Subscribe(OnOpenFriendDetailView);
    }

    public IFriendDetailViewModel FriendDetailViewModel
    {
      get { return _friendDetailViewModel; }
      private set
      {
        _friendDetailViewModel = value;
        OnPropertyChanged();
      }
    }

    public async Task LoadAsync()
    {
      await NavigationViewModel.LoadAsync();
    }

    public INavigationViewModel NavigationViewModel { get; }

    private async void OnOpenFriendDetailView(int friendId)
    {
      if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
      {
        var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
        if (result == MessageDialogResult.Cancel)
        {
          return;
        }
      }

      FriendDetailViewModel = _friendDetailViewModelCreator();
      await FriendDetailViewModel.LoadAsync(friendId);
    }
  }
}
