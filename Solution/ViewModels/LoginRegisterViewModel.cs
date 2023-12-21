using System;
using Solution.Helpers;
using Solution.Services;
using System.Security;
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
            set
            {
                _Navigation = value;
                OnPropertyChanged();
            }
        }

        public NavRelayCommand NavigateToNewTestView { get; set; }
        public ICommand LoginActionButton { get; private set; }
        public ICommand RegisterActionButton { get; private set; }

        public LoginRegisterViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            PropertyChanged += OnPropertyChanged;

            LoginActionButton = new RelayCommand(LoginActionButton_);
            RegisterActionButton = new RelayCommand(RegisterActionButton_);
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
            Console.WriteLine("click login");
            ApiClient api = new ApiClient();
            ApiResponse response = await api.Login(LoginUsername, LoginPassword);

            if (response.Success)
            {
                Console.WriteLine("Navigating away...");
                Navigation.NavigateTo<ScoreViewModel>();
            }
            Console.WriteLine("Staying for whatever reason....");
        }

        private async void RegisterActionButton_()
        {
            Console.WriteLine("click register");
            ApiClient api = new ApiClient();
            ApiResponse response = await api.Register(SignUpUsername, SignUpPassword);

            if (response.Success)
            {
                Navigation.NavigateTo<ScoreViewModel>();
            }else {
                
            }
        }


        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginUsername))
            {
                // Do something with LoginUsername whenever it changes
                // For example, you can access it here:
                // var text = LoginUsername;
            }
            else if (e.PropertyName == nameof(SignUpUsername))
            {
                // Do something with SignUpUsername whenever it changes
                // For example, you can access it here:
                // var text = SignUpUsername;
            }
            else if (e.PropertyName == nameof(LoginPassword))
            {
                // Do something with LoginPassword whenever it changes
                // For example, you can access it here:
                // var secureString = LoginPassword;
            }
            else if (e.PropertyName == nameof(SignUpPassword))
            {
                // Do something with SignUpPassword whenever it changes
                // For example, you can access it here:
                // var secureString = SignUpPassword;
            }
            else if (e.PropertyName == nameof(SignUpConfirmPassword))
            {
                // Do something with SignUpConfirmPassword whenever it changes
                // For example, you can access it here:
                // var secureString = SignUpConfirmPassword;
            }
        }
    }
}