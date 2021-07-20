using System;
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
using BiuBiuShare.Tool;
using HandyControl.Data;
using HandyControl.Tools.Extension;

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

        public ObservableCollection<ChatViewModel> ChatListCollection
            = new ObservableCollection<ChatViewModel>();

        private CollectionViewSource _collectionView
            = new CollectionViewSource();

        private ulong currentTargetId = 0;

        private ChatViewModel currentChatViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _talkService = MagicOnionClient.Create<ITalkService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            _imInfoService = MagicOnionClient.Create<IImInfoService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            _collectionView.Source = ChatListCollection;
            _collectionView.View.Refresh();
            _collectionView.View.SortDescriptions.Add(
                new SortDescription("LastMessageTime"
                    , ListSortDirection.Descending));
            ChatListBox.ItemsSource = _collectionView.View;

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

                    foreach (var chatViewModel in ChatListCollection)
                    {
                        if (chatViewModel.TargetId == message.SourceId)
                        {
                            chatViewModel.ChatInfos.Add(info);
                            chatViewModel.LastMessage = message.Data;
                            chatViewModel.LastMessageTime
                                = IdManagement.TimeGen() << 20;
                            break;
                        }
                    }

                    _collectionView.View.Refresh();
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
                Initialization.Logger.Debug(user.UserId);
                Initialization.Logger.Debug(user.DisplayName);
                Initialization.Logger.Debug(user.Description);
                Initialization.Logger.Debug(user.IconId);
                Initialization.Logger.Debug(user.Email);
                ChatListCollection.Add(
                    new ChatViewModel(user.UserId) { LastMessageTime = IdManagement.TimeGen() << 20 });
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

        private async void SendMessageButton_OnClick(object sender
            , RoutedEventArgs e)
        {
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

            var re = await _talkService.SendMessageAsync(new Message()
            {
                Data = ChatInputbox.Text
                ,
                Type = "Text"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = currentTargetId
            });
            currentChatViewModel.LastMessageTime
                = re.Item1.MessageId;
            currentChatViewModel.LastMessage = ChatInputbox.Text;
            _collectionView.View.Refresh();
            currentChatViewModel.InputData = "";
            ChatInputbox.Text = "";
        }

        private void ChatListBox_OnSelectionChanged(object sender
            , SelectionChangedEventArgs e)
        {
            ChatView.Visibility = Visibility.Visible;
            var chatView = ChatListBox.SelectedItem as ChatViewModel;
            if (chatView is null)
            {
                ChatView.Visibility = Visibility.Hidden;
            }
            else
            {
                ListBoxChat.ItemsSource = chatView.ChatInfos;
                ChatInfos = chatView.ChatInfos;
                currentTargetId = chatView.TargetId;
                ChatInputbox.Text = chatView.InputData;
                currentChatViewModel = chatView;
            }
        }

        private void ChatInputbox_OnTextChanged(object sender
            , TextChangedEventArgs e)
        {
            currentChatViewModel.LastMessageTime
                = IdManagement.TimeGen() << 20;
            currentChatViewModel.InputData = ChatInputbox.Text;
            _collectionView.View.Refresh();
        }
    }
}