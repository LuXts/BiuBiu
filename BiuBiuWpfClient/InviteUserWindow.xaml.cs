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
    }
}