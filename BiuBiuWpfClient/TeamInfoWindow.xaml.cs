using BiuBiuShare.ImInfos;
using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// TeamInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TeamInfoWindow : WindowX, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this
                    , new PropertyChangedEventArgs(propertyName));
        }

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

        private ulong _teamId;

        public string TeamId
        {
            get { return _teamId.ToString(); }
            set
            {
                Notify("TeamId");
            }
        }

        private ulong _ownerId;

        public string OwnerId
        {
            get { return _ownerId.ToString(); }
            set
            {
                Notify("OwnerId");
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

        public TeamInfoWindow()
        {
            InitializeComponent();
            InfoData.DataContext = this;
        }

        public void InitInfo(TeamInfo team, BitmapImage image)
        {
            if (AuthenticationTokenStorage.UserId == team.OwnerId)
            {
                AddUserButton.Visibility = Visibility.Visible;
                this.ModifyButton.IsEnabled = true;
            }
            else
            {
                AddUserButton.Visibility = Visibility.Collapsed;
                this.ModifyButton.IsEnabled = false;
            }
            DisplayName = team.TeamName;
            _teamId = team.TeamId;
            _ownerId = team.OwnerId;
            Description = team.Description;
            BImage = image;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ReadOnly)
            {
                this.Close();
            }
        }

        private void AddUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            InviteUserWindow window = new InviteUserWindow();
            window.Init(_teamId);
            window.Show();
        }

        private void ModifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ModifyButton.Visibility = Visibility.Collapsed;
            this.OKButton.Visibility = Visibility.Collapsed;
            this.SureButton.Visibility = Visibility.Visible;
            this.CancelButton.Visibility = Visibility.Visible;
            ReadOnly = false;
        }

        private async void SureButton_OnClick(object sender, RoutedEventArgs e)
        {
            var teamInfo = await Initialization.DataDb.GetTeamInfoByServer(_teamId);
            teamInfo.Description = Description;
            teamInfo.TeamName = DisplayName;
            if ((await Service.ImInfoService.SetTeamInfo(teamInfo)).Success)
            {
                MessageBoxX.Show("修改成功！");
            }
            else
            {
                MessageBoxX.Show("修改失败！");
            }

            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}