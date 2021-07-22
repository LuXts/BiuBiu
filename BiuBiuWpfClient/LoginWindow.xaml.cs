using BiuBiuShare.ImInfos;
using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;
using System.Windows;
using System.Windows.Input;
using BiuBiuWpfClient.Tools;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : WindowX
    {
        private LiteDBDriven temp;

        public LoginWindow()
        {
            temp = new LiteDBDriven();
            Initialization.GrpcIp = temp.GetIp();
            Initialization.Init();
            InitializeComponent();
            PasswdBox.Password = temp.GetPassword();
            AccountTextBox.Text = temp.GetAccount();

            CheckAccountBox.IsChecked = temp.GetCheckAccount();
            CheckPasswordBox.IsChecked = temp.GetCheckPassword();
            this.Closed += LoginWindow_Closed;
        }

        private void LoginWindow_Closed(object sender, System.EventArgs e)
        {
            if ((bool)CheckAccountBox.IsChecked)
            {
                temp.SetAccount(AccountTextBox.Text);
            }
            if ((bool)CheckPasswordBox.IsChecked)
            {
                temp.SetPassword(PasswdBox.Password);
            }
            temp.SetCheckAccount((bool)CheckAccountBox.IsChecked);
            temp.SetCheckPassword((bool)CheckPasswordBox.IsChecked);
        }

        private void Window_MouseLeftButtonDown(object sender
            , MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonHelper.SetIsPending(LoginButton, true);
            LoginButton.IsEnabled = false;
            string signIn = AccountTextBox.Text;
            string password = PasswdBox.Password;
            var response = await AuthenticationTokenStorage.Login(signIn
                , password, Initialization.GChannel);

            if (response.Success)
            {
                AuthenticationTokenStorage.UserId = response.UserId;
                AuthenticationTokenStorage.DisplayName = response.DisplayName;

                Initialization.ClientFilter
                    = new WithAuthenticationFilter(signIn, password
                        , Initialization.GChannel);

                Service.InitService();
                Initialization.Logger.Debug("sssss");
                var temp = await Initialization.OnlineHub.ConnectAsync(
                    Initialization.GChannel
                    , new UserInfo() { UserId = response.UserId });
                Initialization.Logger.Debug("1sssss1");
                if (temp is null)
                {
                    MessageBoxX.Show("你已经在另一处登录!");
                    LoginButton.IsEnabled = true;
                    ButtonHelper.SetIsPending(LoginButton, false);
                    return;
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Owner = mainWindow;
                this.Close();
            }
            else
            {
                MessageBoxX.Show("登陆失败！");
            }

            LoginButton.IsEnabled = true;
            ButtonHelper.SetIsPending(LoginButton, false);
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Show("请联系管理员电话：139xxxxxxxx", "遇到错误请联系管理员");
        }

        private void IpSettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new IPSettingWindow();
            window.ShowDialog();
        }
    }
}