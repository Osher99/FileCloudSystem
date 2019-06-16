using Client.Infra;
using Client.Services;
using Client.ViewModel;
using Client.Views;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;


namespace Client.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //viewmodels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SignInViewModel>();
            SimpleIoc.Default.Register<RegisterViewModel>();
            SimpleIoc.Default.Register<LobbyViewModel>();
            SimpleIoc.Default.Register<UploadFileViewModel>();

            //services
            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<IFileService, FileService>();

            var navService = new NavigationService();
            navService.Configure("Register", typeof(RegisterView));
            navService.Configure("SignIn", typeof(SignInView));
            navService.Configure("Lobby", typeof(LobbyView));
            navService.Configure("UploadFile", typeof(UploadFileView));

            SimpleIoc.Default.Register<INavigationService>(() => navService);

            var dialog = new Services.DialogService();
            SimpleIoc.Default.Register<IDialogService>(() => dialog);
        }

        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public SignInViewModel SignInVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignInViewModel>();
            }
        }
        public RegisterViewModel RegisterVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterViewModel>();
            }
        }

        public LobbyViewModel LobbyVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LobbyViewModel>();
            }
        }
        public UploadFileViewModel UploadVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UploadFileViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}