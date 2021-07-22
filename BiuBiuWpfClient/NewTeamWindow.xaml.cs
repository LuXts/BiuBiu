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
using BiuBiuShare.ImInfos;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;
using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// NewTeamWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewTeamWindow : WindowX
    {
        public NewTeamWindow()
        {
            InitializeComponent();
        }

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            TeamInfo team = new TeamInfo();
            team.IconId = 1706008201157181464;
            team.Description = Introduction.Text;
            team.TeamName = TeamNameBox.Text;
            var re = await Service.GroFriService.EstablishTeam(
                await Initialization.DataDb.GetUserInfoByServer(
                    AuthenticationTokenStorage.UserId), team);
            if (re.Item1)
            {
                MainWindow.ChatListCollection.Add(
                    new ChatViewModel(re.Item2, team.IconId, team.TeamName));
                InfoViewModel.TeamCollection.Add(new InfoListItem()
                {
                    BImage = await Initialization.DataDb.GetBitmapImage(team.IconId)
                    ,
                    DisplayName = team.TeamName,
                    InfoId = re.Item2,
                    Type = InfoListItem.InfoType.Team
                });
                MessageBoxX.Show("建立群聊成功！");
                this.Close();
            }
            else
            {
                MessageBoxX.Show("建立群聊失败！");
            }
        }
    }
}