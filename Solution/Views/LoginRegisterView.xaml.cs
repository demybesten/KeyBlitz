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
        LoginGrid.Visibility = Visibility.Visible;
        SignUpGrid.Visibility = Visibility.Collapsed;
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
