using BiuBiuShare.GrouFri;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;
using System;
using System.Windows;
using System.Windows.Input;

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

        private void SelectSearch_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9)
                  || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                  || e.Key == Key.Delete || e.Key == Key.Enter
                  || e.Key == Key.Back || e.Key == Key.Tab
                  || e.Key == Key.Right || e.Key == Key.Left))
            {
                e.Handled = true;
            }
        }
    }
}