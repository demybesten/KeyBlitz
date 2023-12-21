using Solution.Helpers;
using Solution.Services;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Solution.ViewModels
{

    public class LoginRegisterViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public INavigationService _Navigation;
        public INavigationService Navigation
        {
            get => _Navigation;
            set { _Navigation = value; OnPropertyChanged(); }
        }

        public NavRelayCommand NavigateToNewTestView { get; set; }
        public ICommand LoginActionButton { get; private set; }
        public ICommand RegisterActionButton { get; private set; }
        private readonly ApiClient apiClient;

        public LoginRegisterViewModel(INavigationService navigation, ApiClient client)
        {
            apiClient = client;
            Navigation = navigation;
            PropertyChanged += OnPropertyChanged;

            LoginActionButton = new RelayCommand(LoginActionButton_);
            RegisterActionButton = new RelayCommand(RegisterActionButton_);
        }

        private string _MessageBlockSignIn = "";
        public string MessageBlockSignIn
        {
            get { return _MessageBlockSignIn; }
            set { SetProperty(ref _MessageBlockSignIn, value); }
        }

        private string _MessageBlockSignUp = "";
        public string MessageBlockSignUp
        {
            get { return _MessageBlockSignUp; }
            set { SetProperty(ref _MessageBlockSignUp, value); }
        }

        private string _loginUsername = "username";
        public string LoginUsername
        {
            get { return _loginUsername; }
            set { SetProperty(ref _loginUsername, value); }
        }

        private string _signUpUsername = "username";
        public string SignUpUsername
        {
            get { return _signUpUsername; }
            set { SetProperty(ref _signUpUsername, value); }
        }

        private string _loginPassword = "password";
        public string LoginPassword
        {
            get { return _loginPassword; }
            set { SetProperty(ref _loginPassword, value); }
        }

        private string _signUpPassword = "password";
        public string SignUpPassword
        {
            get { return _signUpPassword; }
            set { SetProperty(ref _signUpPassword, value); }
        }

        private string _signUpConfirmPassword = "confirm password";
        public string SignUpConfirmPassword
        {
            get { return _signUpConfirmPassword; }
            set { SetProperty(ref _signUpConfirmPassword, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        private async void LoginActionButton_()
        {
            ApiResponse response = await apiClient.Login(LoginUsername, LoginPassword);

            if (response.Success)
            {
                Navigation.NavigateTo<ScoreViewModel>();
            }
            else
            {
                // Handle error
                MessageBlockSignIn = response.Message;
            }
        }

        private async void RegisterActionButton_()
        {
            if (SignUpPassword != SignUpConfirmPassword)
            {
                MessageBlockSignUp = "the passwords are not equal";
                return;
            }

            ApiResponse response = await apiClient.Register(SignUpUsername, SignUpPassword);

            if (response.Success)
            {
                Navigation.NavigateTo<ScoreViewModel>();
            }
            else
            {
                // Handle error
                MessageBlockSignUp = response.Message;
            }
        }
    }
}