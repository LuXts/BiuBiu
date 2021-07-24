using BiuBiuShare.ImInfos;
using Panuon.UI.Silver;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using BiuBiuShare.ImInfos;

using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;

using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// UserInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoWindow : WindowX, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                Notify("DisplayName");
            }
        }

        private bool _readOnly = true;

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                Notify("ReadOnly");
            }
        }

        private ulong _userId;

        public string UserId
        {
            get { return _userId.ToString(); }
            set
            {
                Notify("UserId");
            }
        }

        private string _jobNumber;

        public string JobNumber
        {
            get { return _jobNumber; }
            set
            {
                _jobNumber = value;
                Notify("JobNumber");
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Notify("Description");
            }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                Notify("PhoneNumber");
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                Notify("Email");
            }
        }

        private BitmapImage _bitmap;

        public BitmapImage BImage
        {
            get { return _bitmap; }
            set
            {
                _bitmap = value;
                Notify("BImage");
            }
        }

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this
                    , new PropertyChangedEventArgs(propertyName));
        }

        public UserInfoWindow()
        {
            InitializeComponent();
            InfoData.DataContext = this;
        }

        public void InitInfo(UserInfo user, BitmapImage image)
        {
            if (user.UserId == AuthenticationTokenStorage.UserId)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                DeleteButton.IsEnabled = false;
                ModifyButton.Visibility = Visibility.Visible;
                ModifyButton.IsEnabled = true;
            }
            else
            {
                ModifyButton.Visibility = Visibility.Collapsed;
                ModifyButton.IsEnabled = false;
                DeleteButton.Visibility = Visibility.Visible;
                DeleteButton.IsEnabled = true;
            }
            DisplayName = user.DisplayName;
            _userId = user.UserId;
            _jobNumber = user.JobNumber;
            Description = user.Description;
            _phoneNumber = user.PhoneNumber;
            Email = user.Email;
            BImage = image;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ModifyButton.Visibility = Visibility.Collapsed;
            this.OKButton.Visibility = Visibility.Collapsed;
            this.SureButton.Visibility = Visibility.Visible;
            this.CancelButton.Visibility = Visibility.Visible;
            ReadOnly = false;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ReadOnly)
            {
                this.Close();
            }
        }

        private async void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            var user = await Initialization.DataDb.GetUserInfoByServer(_userId);
            user.Description = Description;
            user.Email = Email;
            user.DisplayName = DisplayName;
            if ((await Service.ImInfoService.SetUserInfo(user)).Success)
            {
                MessageBoxX.Show("修改成功，等待审批！");
            }
            else
            {
                MessageBoxX.Show("修改失败！");
            }
            this.Close();
        }

        private void SureButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserInfo receiver = await Initialization.DataDb.GetUserInfoByServer(_userId);
            ulong sponsorId = AuthenticationTokenStorage.UserId;
            UserInfo sponsor = await Initialization.DataDb.GetUserInfoByServer(sponsorId);

            if (await Service.GroFriService.DeleteFriend(sponsor, receiver))
            {
                MessageBoxX.Show("删除好友成功！");
            }
            else
            {
                MessageBoxX.Show("删除好友失败！");
            }

            foreach (var chat in MainWindow.ChatListCollection)
            {
                if (chat.TargetId == _userId)
                {
                    MainWindow.ChatListCollection.Remove(chat);
                    break;
                }
            }

            foreach (var friend in InfoViewModel.FriendCollection)
            {
                if (friend.InfoId == _userId)
                {
                    InfoViewModel.FriendCollection.Remove(friend);
                    break;
                }
            }

            this.Close();
        }
    }
}