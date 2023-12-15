using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Solution.Views;

public partial class LoginRegisterView : UserControl
{
    public LoginRegisterView()
    {
        InitializeComponent();
        LoginUsername.Text = "username";
        LoginPassword.Text = "password";
        
        SignUpUsername.Text = "username";
        SignUpPassword.Text = "password";
        SignUpConfirmPassword.Text = "confirm password";

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
        if (LoginUsername.Text == "username" || LoginPassword.Text == "password") 
        {
            if (sender == LoginUsername)
            {
                LoginUsername.Text = "";
            }
            if (sender == LoginPassword)
            {
                LoginPassword.Text = "";
            }
            if (sender == SignUpUsername)
            {
                SignUpUsername.Text = "";
            }
            if (sender == SignUpPassword)
            {
                SignUpPassword.Text = "";
            }
            if (sender == SignUpConfirmPassword)
            {
                SignUpConfirmPassword.Text = "";
            }
        }
    }

    public void AddText(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginUsername.Text) || 
            string.IsNullOrWhiteSpace(LoginPassword.Text) ||
            string.IsNullOrWhiteSpace(SignUpUsername.Text) ||
            string.IsNullOrWhiteSpace(SignUpPassword.Text) ||
            string.IsNullOrWhiteSpace(SignUpConfirmPassword.Text))
        {
            if (sender == LoginUsername)
            {
                LoginUsername.Text = "username";
            }
            if (sender == LoginPassword)
            {
                LoginPassword.Text = "password";
            }
            if (sender == SignUpUsername)
            {
                SignUpUsername.Text = "username";
            }
            if (sender == SignUpPassword)
            {
                SignUpPassword.Text = "password";
            }
            if (sender == SignUpConfirmPassword)
            {
                SignUpConfirmPassword.Text = "confirm password";
            }
        }
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