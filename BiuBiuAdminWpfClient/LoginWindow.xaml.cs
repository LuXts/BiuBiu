using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BiuBiuWpfClient.Login;
using Grpc.Net.Client;
using NLog.Fluent;
using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : WindowX
    {
        public LoginWindow()
        {
            new Initialization();
            InitializeComponent();
            PasswdBox.Password = "123456789";
        }

        private void Window_MouseLeftButtonDown(object sender
            , MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;

            string signIn = AccountTextBox.Text;
            string password = PasswdBox.Password;
            var response = await AuthenticationTokenStorage.Login(signIn
                , password, Initialization.GChannel);

            if (response.Success)
            {
                Initialization.ClientFilter = new WithAuthenticationFilter(signIn, password
                    , Initialization.GChannel);

                AuthenticationTokenStorage.UserId = response.UserId;
                AuthenticationTokenStorage.DisplayName = response.DisplayName;

                Service.InitService();
                /*
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Owner = mainWindow;
                this.Close();
                */
            }
            else
            {
                MessageBoxX.Show("登陆失败！");
            }
            LoginButton.IsEnabled = true;
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Show("请联系管理员电话：139xxxxxxxx", "遇到错误请联系管理员");
        }
    }
}