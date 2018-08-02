using FriendOrganizer.UI.Wrapper;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IFriendDetailViewModel : IDetailViewModel
    {
        FriendWrapper Friend { get; }
    }
}