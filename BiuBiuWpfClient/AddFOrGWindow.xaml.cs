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
using BiuBiuShare.GrouFri;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// AddFOrGWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddFOrGWindow : WindowX
    {
        public AddFOrGWindow()
        {
            InitializeComponent();
        }

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            ulong userId = Convert.ToUInt64(SelectSearch.Text);

            var friendRequest = new FriendRequest();
            var teamRequest = new TeamRequest();
            friendRequest.SenderId = AuthenticationTokenStorage.UserId;
            teamRequest.SenderId = AuthenticationTokenStorage.UserId;

            try
            {
                ulong Id = Convert.ToUInt64(this.SelectSearch.Text);
                IdType type = IdManagement.GenerateIdTypeById(Id);
                if (type == IdType.UserId)
                {
                    friendRequest.ReceiverId = Id;
                    friendRequest.RequestMessage = this.Introduction.Text;
                    if ((await Service.GroFriService.AddFriend(friendRequest))
                        .Success)
                    {
                        MessageBox.Show("好友申请发送成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("好友申请发送失败！");
                    }
                }
                else if (type == IdType.TeamId)
                {
                    teamRequest.TeamId = Id;
                    teamRequest.RequestMessage = this.Introduction.Text;
                    if ((await Service.GroFriService.AddGroup(teamRequest))
                        .Success)
                    {
                        MessageBox.Show("群组申请发送成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("群组申请发送失败！");
                    }
                }
                else
                {
                    MessageBox.Show("目标Id类型错误！");
                }
            }
            catch
            {
                MessageBox.Show("出现未知错误！");
            }
        }
    }
}