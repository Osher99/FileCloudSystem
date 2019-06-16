using Client.Infra;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.ViewModel
{

    public class LobbyViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IDialogService _messageService;
        private readonly IFileService _fileService;
        private ObservableCollection<File> _userFiles;
        private User _thisUser;
        private File _thisFile;
        #endregion

        #region Propeties

        public ObservableCollection<File> Files
        {
            get { return _userFiles; }
            set
            {
                _userFiles = value;
                RaisePropertyChanged();
            }
        }
        public User ThisUser
        {
            get { return _thisUser; }
            set
            {
                _thisUser = value;
                RaisePropertyChanged();
            }
        }
        public File SelectedFile
        {
            get { return _thisFile; }
            set
            {
                _thisFile = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand GoToUploadFileView { get; set; }
        public RelayCommand GetFiles { get; set; }
        public RelayCommand SignOutCommand { get; set; }

        public RelayCommand DownloadFileCommand { get; set; }
        public RelayCommand DeleteFileCommand { get; set; }
        public RelayCommand<File> SelectedFileCommand { get; set; }

        #endregion

        public LobbyViewModel(INavigationService navigationService, IUserService userService, IDialogService messageService, IFileService fileService)
        {
            _messageService = messageService;
            _navigationService = navigationService;
            _userService = userService;
            _fileService = fileService;
            InitCommands();
        }

        #region Methods
        private void InitCommands()
        {
            GetFiles = new RelayCommand(async () =>
            {
                Files = new ObservableCollection<File>(await _fileService.GetAllFiles(ThisUser.UserID));
            });

            SelectedFileCommand = new RelayCommand<File>((e) =>
            {
                SelectedFile = e;
            });

            DownloadFileCommand = new RelayCommand(async () =>
            {
                if (SelectedFile != null)
                {
                    if (await _fileService.SaveFile(SelectedFile))
                        await _messageService.ShowMessage("File downloaded successfully!", "Lobby");
                }
                else
                    await _messageService.ShowMessage("Please select a file to download!", "Lobby");

                SelectedFile = null;
            });


            DeleteFileCommand = new RelayCommand(async () =>
            {
                if (SelectedFile != null)
                {
                    if (await _fileService.DeleteFile(SelectedFile.FileID))
                        await _messageService.ShowMessage("File deleted successfully!", "Lobby");
                    else
                        await _messageService.ShowMessage("File deleteion process encountered a problem", "Lobby");
                }

                else
                    await _messageService.ShowMessage("Please select a file to download!", "Lobby");

                Files = new ObservableCollection<File>(await _fileService.GetAllFiles(ThisUser.UserID));
                SelectedFile = null;

            });
            GoToUploadFileView = new RelayCommand(() =>
            {
                SelectedFile = null;

                _navigationService.NavigateTo("UploadFile", ThisUser);
            });

            SignOutCommand = new RelayCommand(() =>
            {
                SelectedFile = null;
                ThisUser = null;
                _navigationService.NavigateTo("SignIn");
            });
        }
    }
}
#endregion