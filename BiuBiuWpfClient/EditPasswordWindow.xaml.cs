using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;
using System.Windows;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// EditPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditPasswordWindow : WindowX
    {
        public EditPasswordWindow()
        {
            InitializeComponent();
        }

        private async void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewPassword1.Password != NewPassword2.Password)
            {
                MessageBoxX.Show("两次密码不一致！");
            }
            else
            {
                var re = await Service.ImInfoService.SetUserPassword(
                    await Initialization.DataDb.GetUserInfoByServer(
                        AuthenticationTokenStorage.UserId), OldPassword.Password
                    , NewPassword1.Password);
                if (re.Success)
                {
                    MessageBoxX.Show("密码修改成功!");
                    this.Close();
                }
                else
                {
                    MessageBoxX.Show("密码修改失败!");
                }
            }
        }
    }
}