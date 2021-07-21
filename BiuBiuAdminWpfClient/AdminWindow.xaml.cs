﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BiuBiuServer.Services;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.UserManagement;
using Grpc.Net.Client;
using HandyControl.Data;

namespace BiuBiuAdminWpfClient
{
    /// <summary>
    /// AdminWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            WinPosition();

            
            List<CategoryInfo1> categoryList1 = new List<CategoryInfo1>();
            categoryList1.Add(new CategoryInfo1 { Name = "用户工号", Value = "jobName" });
            categoryList1.Add(new CategoryInfo1 { Name = "用户Id", Value = "userId" });
            ComboBox1.ItemsSource = categoryList1;
            ComboBox1.DisplayMemberPath = "Name";//显示出来的值
            ComboBox1.SelectedValuePath = "Value";//实际选中后获取的结果的值

            List<CategoryInfo2> categoryList2 = new List<CategoryInfo2>();
            categoryList2.Add(new CategoryInfo2 { Name = "普通用户", Value = "NormalUser" });
            categoryList2.Add(new CategoryInfo2 { Name = "管理员", Value = "Administrator" });
            PermissionsInput.ItemsSource = categoryList2;
            PermissionsInput.DisplayMemberPath = "Name";//显示出来的值
            PermissionsInput.SelectedValuePath = "Value";//实际选中后获取的结果的值

            //this.CheckR3.ItemsSource = UserInfoList;

            // 默认选择查询功能
            Select.IsChecked = true;
        }

        //声明一条注册信息
        public RegisterInfo registerInfo { get; set; }
        //申明一个用户信息列表
        public List<UserInfo> UserInfoList { get; set; }
        public UserInfo userInfo { get; set; }


        //位置偏移
        public void WinPosition()
        {
            double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Top = 120;
            this.Left = 320;
        }

        //下拉框
            public class CategoryInfo1
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public class CategoryInfo2
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        //切换
        private void IsShowSelect(object sender, RoutedEventArgs e)
        {
            this.CheckR2.Visibility = Visibility.Collapsed;
            this.CheckR3.Visibility = Visibility.Collapsed;
            this.AddR.Visibility = Visibility.Collapsed;
            this.SelectR2.Visibility = Visibility.Visible;
            this.SelectR3.Visibility = Visibility.Visible;
        }

        private void IsShowCheck(object sender, RoutedEventArgs e)
        {
            this.AddR.Visibility = Visibility.Collapsed;
            this.SelectR2.Visibility = Visibility.Collapsed;
            this.SelectR3.Visibility = Visibility.Collapsed;
            this.CheckR2.Visibility = Visibility.Visible;
            this.CheckR3.Visibility = Visibility.Visible;

        }

        private void IsShowAdd(object sender, RoutedEventArgs e)
        {

            this.SelectR2.Visibility = Visibility.Collapsed;
            this.SelectR3.Visibility = Visibility.Collapsed;
            this.CheckR2.Visibility = Visibility.Collapsed;
            this.CheckR3.Visibility = Visibility.Collapsed;
            this.AddR.Visibility = Visibility.Visible;
        }

        

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dtOutlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        //点击删除按钮
        private async void ClickAddSure(object sender, RoutedEventArgs e)
        {
            registerInfo = new RegisterInfo();
            registerInfo.JobNumber = this.JobNumInput.Text;
            registerInfo.PhoneNumber = this.PhoneNumInput.Text;
            registerInfo.UserName = this.UserNameInput.Text;
            //MessageBox.Show((string)this.PermissionsInput.SelectedValue);

            if ((string)this.PermissionsInput.SelectedValue== "Administrator")
            {
                registerInfo.Permissions = true;
            }
            else
            {
                registerInfo.Permissions = false;
            }

            int result= await Service.AdminService.RegisteredUsers(registerInfo);

            MessageBox.Show(result.ToString());

            if (result==1)
            {
                MessageBox.Show("注册成功！");
            }
            else if (result==-1)
            {
                MessageBox.Show("注册失败：工号已被注册！");
            }
            else if (result == -2)
            {
                MessageBox.Show("注册失败：手机号已被注册！");
            }
            else
            {
                MessageBox.Show("注册失败！");
            }
        }


        private void SearchBar_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        //点击查询
        private async void ClickSelectSearch(object sender, FunctionEventArgs<string> e)
        {
            UserInfoList = new List<UserInfo>();
            //MessageBox.Show((string)ComboBox1.SelectedValue);
            if ((string)ComboBox1.SelectedValue== "jobName")
            {
                userInfo = await Service.AdminService.SelectByJobNumber(SelectSearch.Text);

                if(userInfo!=null){UserInfoList.Add(userInfo);}
            }
            else if ((string)ComboBox1.SelectedValue == "userId")
            {
                userInfo = await Service.AdminService.SelectByUserId(Convert.ToUInt64((SelectSearch.Text)));
                if (userInfo != null) { UserInfoList.Add(userInfo); }
            }
            SelectR3.ItemsSource = UserInfoList;
        }

        //点击删除
        private async void ClickDeleteButton(object sender, RoutedEventArgs e)
        {

            if (await Service.AdminService.DeleteUser(userInfo.UserId))
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }

        //点击取消
        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            this.JobNumInput.Text = "";
            this.UserNameInput.Text = "";
            this.PhoneNumInput.Text = "";
        }
    }
}
