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
using BiuBiuShare.ImInfos;

namespace BiuBiuAdminWpfClient
{
    /// <summary>
    /// UserInfoDetails.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoDetails : Window
    {
        public UserInfoDetails()
        {
            InitializeComponent();
            WinPosition();
            List<CategoryInfo> category = new List<CategoryInfo>();
            category.Add(new CategoryInfo { Name = "管理员", Value = "Admin" });
            category.Add(new CategoryInfo { Name = "普通用户", Value = "User" });

            ComboBox.ItemsSource = category;
            ComboBox.DisplayMemberPath = "Name";
            ComboBox.SelectedValuePath = "Value";

            InitValue();
        }

        public void WinPosition()
        {
            double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Top = 140;
            this.Left = 390;
        }

        public UserInfo userInfo { get; set; }

        public async void InitValue()
        {
            this.ComboBox.IsEnabled = false;
            this.UserIdInput.IsEnabled = false;
            this.UserNameInput.IsEnabled = false;
            this.EmailInput.IsEnabled = false;
            this.JobNumInput.IsEnabled = false;
            this.PasswordInput.IsEnabled = false;
            this.IntroductionInput.IsEnabled = false;
            this.PhoneNumInput.IsEnabled = false;
            this.ComboBox.IsEnabled = false;
            this.ModifyMessageCancel.Visibility = Visibility.Collapsed;
            this.ModifyPasswordCancel.Visibility = Visibility.Collapsed;
            this.ModifyMessageSure.Visibility = Visibility.Collapsed;
            this.ModifyPasswordSure.Visibility = Visibility.Collapsed;
            this.ModifyPassword.Visibility = Visibility.Visible;
            this.ModifyMessage.Visibility = Visibility.Visible;

            userInfo = await Service.AdminService.SelectByUserId(AdminWindow.userId);
            this.UserIdInput.Text = Convert.ToString((userInfo.UserId));
            this.JobNumInput.Text = userInfo.JobNumber;
            this.UserNameInput.Text = userInfo.DisplayName;
            this.PhoneNumInput.Text = userInfo.PhoneNumber;
            this.EmailInput.Text = userInfo.Email;
            this.PasswordInput.Text = "********";
            this.IntroductionInput.Text = userInfo.Description;
            if (userInfo.Permissions)
            {
                ComboBox.SelectedItem = "管理员";
                ComboBox.SelectedValue = "Admin";
            }
            else
            {
                ComboBox.SelectedItem = "普通用户";
                ComboBox.SelectedValue = "User";
            }

            //MessageBox.Show(userInfo.IconId.ToString());
        }

        public class CategoryInfo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        //当点击修改信息按钮时
        private void ClickModifyMessageButton(object sender, RoutedEventArgs e)
        {
            this.EmailInput.IsEnabled = true;
            this.IntroductionInput.IsEnabled = true;
            this.UserNameInput.IsEnabled = true;
            this.ComboBox.IsEnabled = true;
            this.ModifyPassword.Visibility = Visibility.Collapsed;
            this.ModifyMessage.Visibility = Visibility.Collapsed;
            this.ModifyMessageSure.Visibility = Visibility.Visible;
            this.ModifyMessageCancel.Visibility = Visibility.Visible;
        }

        //当点击修改信息的取消按钮时
        private void ClickModifyMessageCancel(object sender, RoutedEventArgs e)
        {
            InitValue();
        }

        //当点击修改密码的取消按钮时
        private void ClickModifyPasswordCancel(object sender, RoutedEventArgs e)
        {
            InitValue();
        }

        //当点击修改密码的确定按钮时
        private async void ClickModifyPasswordSure(object sender, RoutedEventArgs e)
        {
            if (this.PasswordInput.Text == "")
            {
                MessageBox.Show("密码不能为空！");
                InitValue();
            }
            else
            {
                string password = this.PasswordInput.Text;
                if (await Service.AdminService.ChangePassword(AdminWindow.userId, password))
                {
                    MessageBox.Show("修改密码成功！");
                }
                else
                {
                    MessageBox.Show("修改密码失败！");
                }
                InitValue();
            }
        }

        //当点击修改信息的确定按钮时
        private async void ClickModifyMessageSure(object sender, RoutedEventArgs e)
        {
            userInfo.DisplayName = this.UserNameInput.Text;
            userInfo.Description = this.IntroductionInput.Text;
            userInfo.Email = this.EmailInput.Text;
            userInfo.PhoneNumber = this.PhoneNumInput.Text;
            userInfo.Permissions = (string)this.ComboBox.SelectedValue == "Admin" ? true : false;
            //MessageBox.Show(userInfo.IconId.ToString());
            if (await Service.AdminService.ChangeUserInfo(userInfo))
            {
                MessageBox.Show("修改基本信息成功！");
            }
            else
            {
                MessageBox.Show("修改基本信息失败！");
            }
            InitValue();
        }

        //当点击修改按钮时
        private void ClickModifyPassword(object sender, RoutedEventArgs e)
        {
            this.PasswordInput.IsEnabled = true;
            this.PasswordInput.Text = "";
            this.ModifyPassword.Visibility = Visibility.Collapsed;
            this.ModifyMessage.Visibility = Visibility.Collapsed;
            this.ModifyPasswordSure.Visibility = Visibility.Visible;
            this.ModifyPasswordCancel.Visibility = Visibility.Visible;
        }
    }
}