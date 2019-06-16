using Client.Infra;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{

    public class RegisterViewModel : ViewModelBase
    {
        #region Fields
        INavigationService _navigationService;
        IUserService _userService;
        IDialogService _messageService;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private User _newUser;

        #endregion

        #region Propeties

        public User NewUser
        {
            get { return _newUser; }
            set
            {
                _newUser = value;
                RaisePropertyChanged();

            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged();

            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged();

            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged();

            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();

            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged();

            }
        }
        #endregion

        #region Commands
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand GoToSignInView { get; set; }
        #endregion

        public RegisterViewModel(INavigationService navigationService, IUserService userService, IDialogService messageService)
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Password = "";
            ConfirmPassword = "";
            NewUser = null;
            _messageService = messageService;
            _navigationService = navigationService;
            _userService = userService;
            InitCommands();
        }

        #region Methods
        private void InitCommands()
        {
            RegisterCommand = new RelayCommand(async () =>
            {

                if (Email == "" || Password == "" || FirstName == "" || LastName == "" || ConfirmPassword == "")
                {
                    await _messageService.ShowMessage("ALL Fields are required!!", "Invalid Input!");
                    return;
                }
                if (Password.Length < 4 || Email.Length < 4)
                {
                    await _messageService.ShowMessage("Username & Password Must Have Atleast 4 Characters", "Invalid Input!");
                    return;
                }
                if (FirstName.Length < 3 || LastName.Length < 3)
                {
                    await _messageService.ShowMessage("FirstName & LastName Must Have Atleast 3 Characters", "Invalid Input!");
                    return;
                }
                if (Password != ConfirmPassword)
                {
                    await _messageService.ShowMessage("Make Sure to CONFIRM the RIGHT Password!", "Invalid Input!");
                    return;
                }
                User newUser = new User()
                {
                    Email = this.Email,
                    Password = this.Password,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                };

                var registeredUser = await _userService.RegisterAsync(newUser);
                if (registeredUser == null)
                {
                    await _messageService.ShowMessage("Registerition encountered a problem", "Register Window");

                }
                else
                {
                    await _messageService.ShowMessage("You Successfully Registered!", "Register Window");
                    this.Email = "";
                    this.Password = "";
                    this.FirstName = "";
                    this.LastName = "";
                    _navigationService.NavigateTo("Lobby", registeredUser);
                }
            });

            GoToSignInView = new RelayCommand(() =>
            {
                this.Email = "";
                this.Password = "";
                this.FirstName = "";
                this.LastName = "";
                _navigationService.NavigateTo("SignIn");
            });
        }
    }
}
#endregion