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
    public class SignInViewModel : ViewModelBase
    {
        #region Members
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IDialogService _messageService;
        private string _email;
        private string _password;
        #endregion

        #region Properties

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }



        #endregion

        #region Commands
        public RelayCommand SignInUserCommand { get; set; }
        public RelayCommand GoToRegisterView { get; set; }
        #endregion

        public SignInViewModel(INavigationService navigationService, IUserService userService, IDialogService messageService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _messageService = messageService;
            InitCommands();
        }

        #region Methods
        public void InitCommands()
        {
            SignInUserCommand = new RelayCommand(async () =>
            {
                if (Email == "" || Email == null || Password == "" || Password == null)
                {
                    await _messageService.ShowMessage("Username & Password are required!", "Invalid Input");
                    return;
                }
                if (Password.Length < 4 || Email.Length < 4)
                {
                    await _messageService.ShowMessage("Username & Password Must Have Atleast 4 Characters", "Invalid Input");
                    return;
                }

                var newUser = new User { Email = this.Email, Password = this.Password };
                var isFound = await _userService.SignInAsync(newUser);
                if (isFound != null)
                {
                    await _messageService.ShowMessage("You Successfully Logged In!", "Login Window");
                    _navigationService.NavigateTo("Lobby", isFound);

                }
                else
                {
                    await _messageService.ShowMessage("Sign in encountered a problem, try again", "Login Window");

                }
                Email = "";
                Password = "";

            });

        
            GoToRegisterView = new RelayCommand(() =>
            {
                Email = "";
                Password = "";
                _navigationService.NavigateTo("Register");
            });
        }
        #endregion
    }
}
