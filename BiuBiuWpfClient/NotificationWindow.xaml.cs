using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Model;
using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// NotificationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationWindow : WindowX, INotifyPropertyChanged
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

        public NotificationWindow()
        {
            InitializeComponent();
            InfoData.DataContext = this;
        }

        private ulong _id;

        public void Init(string displayName, string description, BitmapImage bImage, ulong id)
        {
            _id = id;
            DisplayName = displayName;
            Description = description;
            BImage = bImage;
        }

        private async void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool mark;
            if (IdManagement.GenerateIdTypeById(_id) == IdType.FriendRequestId)
            {
                var request = Initialization.DataDb.GetFriendRequest(_id);
                var re = await Service.GroFriService.ReplyFriendRequest(request
                    , true);
                mark = re.Success;
                if (mark)
                {
                    foreach (var item in InfoViewModel.NewFriendCollection)
                    {
                        if (item.InfoId == _id)
                        {
                            InfoViewModel.NewFriendCollection.Remove(item);
                            break;
                        }
                    }
                }
            }
            else if (IdManagement.GenerateIdTypeById(_id) ==
                      IdType.TeamInvitationId)
            {
                var request = Initialization.DataDb.GetTeamInvitation(_id);
                var re = await Service.GroFriService.ReplyGroupInvitation(request
                    , true);
                mark = re.Success;
                if (mark)
                {
                    foreach (var item in InfoViewModel.TeamInvitationCollection)
                    {
                        if (item.InfoId == _id)
                        {
                            InfoViewModel.TeamInvitationCollection.Remove(item);
                            break;
                        }
                    }
                }
            }
            else if (IdManagement.GenerateIdTypeById(_id) ==
                   IdType.TeamRequestId)
            {
                var request = Initialization.DataDb.GetTeamRequest(_id);
                var re = await Service.GroFriService.ReplyGroupRequest(request
                    , true);
                mark = re.Success;
                if (mark)
                {
                    foreach (var item in InfoViewModel.TeamRequestCollection)
                    {
                        if (item.InfoId == _id)
                        {
                            InfoViewModel.TeamRequestCollection.Remove(item);
                            break;
                        }
                    }
                }
            }
            else
            {
                mark = false;
            }

            if (!mark)
            {
                MessageBoxX.Show("发送失败！");
            }
            this.Close();
        }

        private async void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool mark;
            if (IdManagement.GenerateIdTypeById(_id) == IdType.FriendRequestId)
            {
                var request = Initialization.DataDb.GetFriendRequest(_id);
                var re = await Service.GroFriService.ReplyFriendRequest(request
                    , false);
                mark = re.Success;
            }
            else if (IdManagement.GenerateIdTypeById(_id) ==
                     IdType.TeamInvitationId)
            {
                var request = Initialization.DataDb.GetTeamInvitation(_id);
                var re = await Service.GroFriService.ReplyGroupInvitation(request
                    , false);
                mark = re.Success;
            }
            else if (IdManagement.GenerateIdTypeById(_id) ==
                     IdType.TeamRequestId)
            {
                var request = Initialization.DataDb.GetTeamRequest(_id);
                var re = await Service.GroFriService.ReplyGroupRequest(request
                    , false);
                mark = re.Success;
            }
            else
            {
                mark = false;
            }
            if (!mark)
            {
                MessageBoxX.Show("发送失败！");
            }
            this.Close();
        }
    }
}