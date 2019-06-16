using Client.Infra;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class UploadFileViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IDialogService _messageService;
        private readonly IFileService _fileService;
        private User _thisUser;
        private File _newFile;
        private string _description;
        #endregion

        #region Properties
        public User ThisUser
        {
            get { return _thisUser; }
            set
            {
                _thisUser = value;
                RaisePropertyChanged();
            }
        }
        public File NewFile
        {
            get { return _newFile; }
            set
            {
                _newFile = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }


        #endregion

        #region Commands
        public RelayCommand BrowseFileCommand { get; set; }
        public RelayCommand UploadFileCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand GoToListFilesView { get; set; }
        #endregion

        public UploadFileViewModel(INavigationService navigationService, IUserService userService, IDialogService messageService, IFileService fileService)
        {
            _messageService = messageService;
            _navigationService = navigationService;
            _userService = userService;
            _fileService = fileService;
            ThisUser = new User();
            NewFile = new File();
            Description = "";

            BrowseFileCommand = new RelayCommand(async () =>
            {
                NewFile = await _fileService.Browse();
                if (NewFile != null)
                {
                    NewFile.UserID = ThisUser.UserID;
                    NewFile.Description = Description;
                }
            });
            UploadFileCommand = new RelayCommand(async () =>
            {
                if (NewFile != null)
                {
                    if (await _fileService.UploadFile(NewFile))
                    {
                        await _messageService.ShowMessage("File uploaded successfully!", "UploadFileWindows");
                        Description = "";
                        NewFile = null;
                    }
                    else
                        await _messageService.ShowMessage("Uploaded process is stopped due to error", "UploadFileWindows");
                }            
                else
                    await _messageService.ShowMessage("Please browse a file before trying to upload", "UploadFileWindows");        
            }, () => NewFile != null);
            SignOutCommand = new RelayCommand(() =>
            {
                ThisUser = new User();
                NewFile = new File();
                Description = "";
                _navigationService.NavigateTo("SignIn");
            });
            GoToListFilesView = new RelayCommand(() =>
            {
                NewFile = null;
                Description = "";
                _navigationService.NavigateTo("Lobby", ThisUser);
                ThisUser = new User();
            });
        }
    }
}