using System;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;
using MagicOnion.Client;
using Panuon.UI.Silver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BiuBiuServer.Userhub;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Tool;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Microsoft.Win32;

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

        public BitmapImage MyHeadIcon { get; set; }

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

            _userHubClient.SMEvent += async (Message message) =>
            {
                object data;
                ChatInfoType type;
                if (message.Type == "Text")
                {
                    data = message.Data;
                    type = ChatInfoType.TextTypeChat;
                }
                else if (message.Type == "Image")
                {
                    data = await Initialization.DataDb.GetBitmapImage(
                        message.MessageId);
                    type = ChatInfoType.ImageTypeChat;
                }
                else
                {
                    data = message.Data;
                    type = ChatInfoType.TextTypeChat;
                }
                foreach (var chatViewModel in ChatListCollection)
                {
                    if (chatViewModel.TargetId == message.SourceId)
                    {
                        var info = new ChatInfoModel
                        {
                            Message = data
                            ,
                            SenderId = message.SourceId.ToString()
                            ,
                            Type = type
                            ,
                            Role = TypeLocalMessageLocation.chatRecv
                            ,
                            MessageOnwer = chatViewModel.DisplayName
                            ,
                            BImage = chatViewModel.BImage
                        };
                        Initialization.Logger.Debug(chatViewModel
                            .DisplayName);
                        chatViewModel.ChatInfos.Add(info);
                        chatViewModel.LastMessage = message.Data;
                        chatViewModel.LastMessageTime
                            = IdManagement.TimeGen() << 20;
                        break;
                    }
                }
                _collectionView.View.Refresh();
            };

            this.Closed += MainWindow_Closed;
            InitChat();
        }

        private async void InitChat()
        {
            var myInfo = await _imInfoService.GetUserInfo(new UserInfo()
            {
                UserId = AuthenticationTokenStorage.UserId
            });

            MyHeadIcon
                = await Initialization.DataDb.GetBitmapImage(myInfo.IconId);

            var userInfos = await _imInfoService.GetUserFriendsId(
                new UserInfo() { UserId = AuthenticationTokenStorage.UserId });
            foreach (var user in userInfos)
            {
                var chat = new ChatViewModel(user.UserId)
                {
                    LastMessageTime = IdManagement.TimeGen() << 20
                };
                ChatListCollection.Add(chat);
                chat.ChatInfos.CollectionChanged += ListBox_SourceUpdated;
            }
        }

        private void ListBox_SourceUpdated(object sender, EventArgs e)
        {
            Decorator decorator
                = (Decorator)VisualTreeHelper.GetChild(ListBoxChat, 0);
            ScrollViewer scrollViewer = (ScrollViewer)decorator.Child;
            scrollViewer.ScrollToEnd();
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
                Type = ChatInfoType.TextTypeChat
                ,
                Role = TypeLocalMessageLocation.chatSend
                ,
                BImage = MyHeadIcon
                ,
                MessageOnwer = AuthenticationTokenStorage.DisplayName
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

            currentChatViewModel.LastMessageTime = re.Item1.MessageId;
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
            currentChatViewModel.LastMessageTime = IdManagement.TimeGen() << 20;
            currentChatViewModel.InputData = ChatInputbox.Text;
            _collectionView.View.Refresh();
        }

        private async void LoadImageButton_OnClick(object sender
            , RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (File.Exists(fileName))
                {
                    var re = await Initialization.DataDb.SendImageToServer(
                        currentTargetId, fileName);
                    Initialization.Logger.Debug(re.Success);
                    if (re.Success)
                    {
                        var image
                            = await Initialization.DataDb
                                .GetBitmapImage(re.MessageId);
                        var info = new ChatInfoModel
                        {
                            Message
                                = image
                            ,
                            SenderId
                                = AuthenticationTokenStorage.UserId
                                    .ToString()
                            ,
                            Type = ChatInfoType.ImageTypeChat
                            ,
                            Role = TypeLocalMessageLocation.chatSend
                            ,
                            BImage = MyHeadIcon
                            ,
                            MessageOnwer = AuthenticationTokenStorage
                                .DisplayName
                        };
                        ChatInfos.Add(info);
                    }
                }
                else
                {
                    MessageBoxX.Show("文件不存在！");
                }
            }
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                ListBoxItem temp = sender as ListBoxItem;
                ChatInfoModel chatInfoModel = temp?.DataContext as ChatInfoModel;
                if (chatInfoModel?.Type == ChatInfoType.ImageTypeChat)
                {
                }
            }
        }
    }
}