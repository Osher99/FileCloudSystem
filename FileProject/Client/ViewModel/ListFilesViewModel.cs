using Client.Infra;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class ListFilesViewModel : ViewModelBase
    {
        #region Fields
        INavigationService _navigationService;
        IUserService _userService;
        IDialogService _messageService;
        IFileService _fileService;
        private ObservableCollection<File> _files;
        private User _myUser;
        public File _selectedFile { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<File> Files
        {
            get { return _files; }
            set { _files = value; RaisePropertyChanged(); }
        }


        public File SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; RaisePropertyChanged(); }
        }

        public User MyUser
        {
            get { return _myUser; }
            set { _myUser = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Commands
        public RelayCommand GetFilesList { get; set; }
        public RelayCommand GoToUploadView { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand DownloadFileCommand { get; set; }
        public RelayCommand<File> SelectFileCommand { get; set; }
        #endregion

        public ListFilesViewModel(INavigationService navigationService, IUserService userService, IDialogService messageService, IFileService fileService)
        {
            _messageService = messageService;
            _navigationService = navigationService;
            _userService = userService;
            _fileService = fileService;
            MyUser = new User();
            SelectedFile = null;
            InitCommands();
        }

        private void InitCommands()
        {
            GetFilesList = new RelayCommand(async () =>
            {
                IEnumerable<File> listofFiles = await _fileService.GetFileList(MyUser.Token);
                if (listofFiles != null)
                    Files = new ObservableCollection<File>(listofFiles);
            });

            GoToUploadView = new RelayCommand(() =>
            {
                _navigationService.NavigateTo("UploadFile", MyUser);
            });
            SignOutCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo("SignIn");
            });
            DownloadFileCommand = new RelayCommand(async () =>
            {
                if (SelectedFile != null)
                {
                    var FileFromDB = await _fileService.DownloadFile(SelectedFile.FileID, MyUser.Token);
                    if(FileFromDB != null)
                    {
                        bool a = await _fileService.SaveFile(FileFromDB);
                        if(a)
                            await _messageService.ShowMessage("Downloaded Successfully", "Success");
                        else
                            await _messageService.ShowMessage("Can't Save File", "Error");
                    }
                    else
                        await _messageService.ShowMessage("Can't Download File", "Error");
                }
                else
                    await _messageService.ShowMessage("Can't Download File", "Error");
            });
            SelectFileCommand = new RelayCommand<File>((file) =>
            {
                SelectedFile = file;
            });


        }
    }
}
