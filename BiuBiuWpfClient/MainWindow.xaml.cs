using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;
using MagicOnion.Client;
using Panuon.UI.Silver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BiuBiuServer.Userhub;
using BiuBiuShare.ImInfos;
using HandyControl.Data;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        private ITalkService _talkService;
        private IImInfoService _imInfoService;
        private UserHubClient _userHubClient;

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; }
            = new ObservableCollection<ChatInfoModel>();

        public ObservableCollection<ChatViewModel> ChatListCollection = new ObservableCollection<ChatViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            _talkService = MagicOnionClient.Create<ITalkService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            _imInfoService = MagicOnionClient.Create<IImInfoService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            ListBoxChat.ItemsSource = ChatInfos;

            var view = CollectionViewSource.GetDefaultView(ChatListCollection);
            view.SortDescriptions.Add(new SortDescription("LastMessageTime"
                , ListSortDirection.Descending));

            ChatListBox.ItemsSource = view;

            _userHubClient = new UserHubClient();
            _userHubClient.ConnectAsync(Initialization.GChannel
                , AuthenticationTokenStorage.UserId);

            _userHubClient.SMEvent += (Message message) =>
            {
                if (message.Type == "Text")
                {
                    var info = new ChatInfoModel
                    {
                        Message = message.Data
                        ,
                        SenderId = message.SourceId.ToString()
                        ,
                        Type = ChatMessageType.String
                        ,
                        Role = ChatRoleType.Receiver
                    };
                    this.ChatInfos.Add(info);
                }
            };

            this.Closed += MainWindow_Closed;
            InitChat();
        }

        private async void InitChat()
        {
            var userInfos = await _imInfoService.GetUserFriendsId(
                new UserInfo() { UserId = AuthenticationTokenStorage.UserId });
            foreach (var user in userInfos)
            {
                ChatListCollection.Add(
                    new ChatViewModel(user.UserId) { LastMessageTime = 0 });
            }
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            _userHubClient.DisposeAsync();
            _userHubClient.WaitForDisconnect();
        }

        private void MainWindow_OnSizeChanged(object sender
            , SizeChangedEventArgs e)
        {
            RowDefinition.MaxHeight = this.Height / 2;
        }

        private void SendMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            ulong TargetId;
            if (AuthenticationTokenStorage.UserId == 1705766111136452612)
            {
                TargetId = 1705766111094509568;
            }
            else
            {
                TargetId = 1705766111136452612;
            }

            var info = new ChatInfoModel
            {
                Message = ChatInputbox.Text
                ,
                SenderId = AuthenticationTokenStorage.UserId.ToString()
                ,
                Type = ChatMessageType.String
                ,
                Role = ChatRoleType.Sender
            };
            ChatInfos.Add(info);

            var re = _talkService.SendMessageAsync(new Message()
            {
                Data = ChatInputbox.Text
                ,
                Type = "Text"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = TargetId
            });

            ChatInputbox.Text = "";
        }

        private void ChatListBox_OnSelectionChanged(object sender
            , SelectionChangedEventArgs e)
        {
            MessageBoxX.Show(ChatListBox.SelectedIndex.ToString());
            Initialization.Logger.Debug(ChatListBox.SelectedItem.GetType());
        }
    }
}