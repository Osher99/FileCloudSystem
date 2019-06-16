using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        private RelayCommand _loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("SignIn");
                    }));
            }
        }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}