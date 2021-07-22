using BiuBiuShare.ImInfos;
using Panuon.UI.Silver;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

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
            ReadOnly = false;
        }

        private async void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ReadOnly)
            {
                this.Close();
            }
        }
    }
}