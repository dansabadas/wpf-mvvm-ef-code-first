using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        private string _title;
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;

        public DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        public int Id { get; protected set; }

        public string Title
        {
            get => _title;
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public abstract Task LoadAsync(int? id);

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand CloseDetailViewCommand { get; }

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    RaiseCanExecuteChanged(SaveCommand);
                }
            }
        }

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new
                AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = GetType().Name
            });
        }

        protected virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = MessageDialogService.ShowOkCancelDialog("You've made changes. Close this item?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }
    }
}
