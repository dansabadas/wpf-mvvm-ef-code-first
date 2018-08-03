using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
  public class NavigationItemViewModel : ViewModelBase
  {
    private string _displayMember;
    private readonly IEventAggregator _eventAggregator;
    private readonly string _detailViewModelName;

    public NavigationItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator)
    {
      _eventAggregator = eventAggregator;
      _detailViewModelName = detailViewModelName;

      Id = id;
      DisplayMember = displayMember;
      OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
    }

    public int Id { get; }

    public string DisplayMember
    {
      get => _displayMember;
      set
      {
        _displayMember = value;
        OnPropertyChanged();
      }
    }

    public ICommand OpenDetailViewCommand { get; }

    private void OnOpenDetailViewExecute()
    {
      _eventAggregator
        .GetEvent<OpenDetailViewEvent>()
        .Publish(new OpenDetailViewEventArgs
          {
              Id = Id, 
              ViewModelName = _detailViewModelName
          });
    }
  }
}
