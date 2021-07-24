using BiuBiuShare.GrouFri;
using BiuBiuShare.Tool;
using Panuon.UI.Silver;
using System;
using System.Windows;
using System.Windows.Input;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// InviteUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InviteUserWindow : WindowX
    {
        public InviteUserWindow()
        {
            InitializeComponent();
        }

        private ulong _teamId;

        public void Init(ulong teamId)
        {
            _teamId = teamId;
        }

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            ulong userId = Convert.ToUInt64(SelectSearch.Text);

            var teamInvitation = new TeamInvitation();
            teamInvitation.TeamId = _teamId;
            teamInvitation.InvitationMessage = Introduction.Text;
            teamInvitation.ReceiverId = userId;

            try
            {
                ulong Id = Convert.ToUInt64(this.SelectSearch.Text);
                IdType type = IdManagement.GenerateIdTypeById(Id);
                if (type == IdType.UserId)
                {
                    if ((await Service.GroFriService.InviteUserToGroup(teamInvitation))
                        .Success)
                    {
                        MessageBoxX.Show("入群邀请发送成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxX.Show("入群邀请发送失败！");
                    }
                }
                else
                {
                    MessageBoxX.Show("目标Id类型错误！");
                }
            }
            catch
            {
                MessageBoxX.Show("出现未知错误！");
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