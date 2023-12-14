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

        LoginUsername.GotFocus += RemoveText;
        LoginUsername.LostFocus += AddText;
        LoginPassword.GotFocus += RemoveText;
        LoginPassword.LostFocus += AddText;
        
        SignUpUsername.GotFocus += RemoveText;
        SignUpUsername.LostFocus += AddText;
        SignUpPassword.GotFocus += RemoveText;
        SignUpPassword.LostFocus += AddText;
        
        LoginGrid.Visibility = Visibility.Visible;
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
        }
    }

    public void AddText(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginUsername.Text) || string.IsNullOrWhiteSpace(LoginPassword.Text))
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