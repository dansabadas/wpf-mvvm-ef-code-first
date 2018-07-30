using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private readonly IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;

        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ICommand DeleteCommand { get; set; }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _friendRepository.Remove(Friend.Model);
                await _friendRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggregator
                .GetEvent<AfterFriendSavedEvent>()
                .Publish(
                    new AfterFriendSavedEventArgs
                    {
                        Id = Friend.Id,
                        DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                    });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
                ? await _friendRepository.GetByIdAsync(friendId.Value)
                : CreateNewFriend();

            InitializeFriend(friend);
            
            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    RaiseCanExecuteChanged(SaveCommand);
                }
            };
            RaiseCanExecuteChanged(SaveCommand);
            if (Friend.Id == 0)
            {
                // Little trick to trigger the validation
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "-" });
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public FriendWrapper Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public bool HasChanges
        {
            get => _hasChanges;
            private set
            {
                if (_hasChanges == value)
                {
                    return;
                }

                _hasChanges = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged(SaveCommand);
            }
        }
    }
}
