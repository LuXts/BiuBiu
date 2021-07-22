using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BiuBiuAdminWpfClient
{
    /// <summary>
    /// CheckInfoDetails.xaml 的交互逻辑
    /// </summary>
    public partial class CheckInfoDetails : Window
    {
        public CheckInfoDetails()
        {
            InitializeComponent();
            WinPosition();
            this.Email.Text = AdminWindow.userInfoForCheck.Email;
            this.Introduction.Text = AdminWindow.userInfoForCheck.Description;
            this.UserName.Text = AdminWindow.userInfoForCheck.DisplayName;
        }

        public void WinPosition()
        {
            double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Top = 160;
            this.Left = 360;
        }

        //当点击审批通过时
        private async void ClickAccept(object sender, RoutedEventArgs e)
        {
            if (await Service.AdminService.ReviewMessage(AdminWindow.userInfoForCheck.UserId, true))
            {
                MessageBox.Show("审批成功！");
            }
            else
            {
                MessageBox.Show("审批失败！");
            }
            this.Close();
        }

        //当点击审批拒绝时
        private async void ClickReject(object sender, RoutedEventArgs e)
        {
            if (await Service.AdminService.ReviewMessage(AdminWindow.userInfoForCheck.UserId, false))
            {
                MessageBox.Show("审批成功！");
            }
            else
            {
                MessageBox.Show("审批失败！");
            }
            this.Close();
        }
    }
}