namespace HyberShift_CSharp.ViewModel
{
    public class SignInPageViewModel : BaseViewModel
    {
        private object selectedViewModel;

        public SignInPageViewModel()
        {
            SelectedViewModel = new LoginViewModel(ViewModelNavigator);
            //SelectedViewModel = new RegisterViewModel(RegisterVMNavigator);
        }

        //public LoginViewModel LoginVM { get; set; }
        //public RegisterViewModel RegisterVM { get; set; }

        public object SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                NotifyChanged("SelectedViewModel");
            }
        }

        public void ViewModelNavigator(object obj)
        {
            if (obj.ToString() == "RegisterViewModel") SelectedViewModel = new RegisterViewModel(ViewModelNavigator);
            if (obj.ToString() == "LoginViewModel") SelectedViewModel = new LoginViewModel(ViewModelNavigator);
        }
    }
}