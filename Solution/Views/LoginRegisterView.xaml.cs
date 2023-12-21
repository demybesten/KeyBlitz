using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Solution.Services;

namespace Solution.Views;

public partial class LoginRegisterView : UserControl
{
    public LoginRegisterView()
    {
        InitializeComponent();
        LoginUsername.Text = "username";
        LoginPassword.Password = "password";

        SignUpUsername.Text = "username";
        SignUpPassword.Password = "password";
        SignUpConfirmPassword.Password = "confirm password";

        LoginUsername.GotFocus += RemoveText;
        LoginUsername.LostFocus += AddText;
        LoginPassword.GotFocus += RemoveText;
        LoginPassword.LostFocus += AddText;

        SignUpUsername.GotFocus += RemoveText;
        SignUpUsername.LostFocus += AddText;
        SignUpPassword.GotFocus += RemoveText;
        SignUpPassword.LostFocus += AddText;
        SignUpConfirmPassword.GotFocus += RemoveText;
        SignUpConfirmPassword.LostFocus += AddText;

        LoginGrid.Visibility = Visibility.Visible;
        SignUpGrid.Visibility = Visibility.Collapsed;
    }

    public void RemoveText(object sender, EventArgs e)
    {
        if (LoginUsername.Text == "username" || LoginPassword.Password == "password")
        {
            if (sender == LoginUsername)
            {
                LoginUsername.Text = "";
            }
            if (sender == LoginPassword)
            {
                LoginPassword.Password = "";
            }
            if (sender == SignUpUsername)
            {
                SignUpUsername.Text = "";
            }
            if (sender == SignUpPassword)
            {
                SignUpPassword.Password = "";
            }
            if (sender == SignUpConfirmPassword)
            {
                SignUpConfirmPassword.Password = "";
            }
        }
    }

    public void AddText(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginUsername.Text) ||
            string.IsNullOrWhiteSpace(LoginPassword.Password) ||
            string.IsNullOrWhiteSpace(SignUpUsername.Text) ||
            string.IsNullOrWhiteSpace(SignUpPassword.Password) ||
            string.IsNullOrWhiteSpace(SignUpConfirmPassword.Password))
        {
            if (sender == LoginUsername)
            {
                LoginUsername.Text = "username";
            }
            if (sender == LoginPassword)
            {
                LoginPassword.Password = "password";
            }
            if (sender == SignUpUsername)
            {
                SignUpUsername.Text = "username";
            }
            if (sender == SignUpPassword)
            {
                SignUpPassword.Password = "password";
            }
            if (sender == SignUpConfirmPassword)
            {
                SignUpConfirmPassword.Password = "confirm password";
            }
        }
    }

    private async void LoginActionButton(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("click login");
        ApiClient api = new ApiClient();
        ApiResponse response = await api.Login(LoginUsername.Text, LoginPassword.Password);
    }

    private async void RegisterActionButton(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("click register");
        ApiClient api = new ApiClient();
        ApiResponse response = await api.Register(SignUpUsername.Text, SignUpPassword.Password);
    }

    private void LoginButtonClick(object sender, RoutedEventArgs e)
    {
        LoginGrid.Visibility = Visibility.Visible;
        SignUpGrid.Visibility = Visibility.Collapsed;
        ToLogInScreen.Foreground = new SolidColorBrush(Colors.White);
        ToSignUpScreen.Foreground = new SolidColorBrush(Colors.Gray);
    }

    private void SignUpButtonClick(object sender, RoutedEventArgs e)
    {
        SignUpGrid.Visibility = Visibility.Visible;
        LoginGrid.Visibility = Visibility.Collapsed;
        ToLogInScreen.Foreground = new SolidColorBrush(Colors.Gray);
        ToSignUpScreen.Foreground = new SolidColorBrush(Colors.White);
    }
}