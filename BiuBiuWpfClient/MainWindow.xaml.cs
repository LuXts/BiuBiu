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
using BiuBiuShare.ImInfos;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Tools;
using BiuBiuWpfClient.Userhub;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using ScrollViewer = System.Windows.Controls.ScrollViewer;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; }
            = new ObservableCollection<ChatInfoModel>();

        public ObservableCollection<ChatViewModel> ChatListCollection
            = new ObservableCollection<ChatViewModel>();

        public static CollectionViewSource CollectionView
            = new CollectionViewSource();

        private UserHubClient _userHubClient;

        private ulong currentTargetId = 0;

        private ChatViewModel currentChatViewModel;

        public BitmapImage MyHeadIcon { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            CollectionView.Source = ChatListCollection;
            CollectionView.View.Refresh();
            CollectionView.View.SortDescriptions.Add(
                new SortDescription("LastMessageTime"
                    , ListSortDirection.Descending));
            ChatListBox.ItemsSource = CollectionView.View;

            _userHubClient = new UserHubClient();
            _userHubClient.ConnectAsync(Initialization.GChannel
                , AuthenticationTokenStorage.UserId);

            _userHubClient.SMEvent += async (Message message) =>
            {
                var temp = await MessageToChatInfo.TransformChatInfoModel(message);
                string tip;
                if (message.Type == "Text")
                {
                    tip = message.Data;
                }
                else if (message.Type == "Image")
                {
                    tip = "[图片]";
                }
                else
                {
                    tip = "[文件]";
                }

                foreach (var chatViewModel in ChatListCollection)
                {
                    if (chatViewModel.TargetId == message.SourceId)
                    {
                        if (chatViewModel != currentChatViewModel)
                        {
                            chatViewModel.NoReadNumber += 1;
                        }

                        temp.BImage = chatViewModel.BImage;
                        temp.MessageOnwer = chatViewModel.DisplayName;
                        chatViewModel.ChatInfos.Add(temp);
                        chatViewModel.LastMessage = tip;
                        chatViewModel.LastMessageTime
                            = IdManagement.TimeGen() << 20;
                        break;
                    }
                }

                CollectionView.View.Refresh();
            };

            this.Closed += MainWindow_Closed;
            InitChat();
        }

        private async void InitChat()
        {
            var myInfo
                = await Initialization.DataDb.GetUserInfoByServer(
                    AuthenticationTokenStorage.UserId);
            MyHeadIcon
                = await Initialization.DataDb.GetBitmapImage(myInfo.IconId);

            var userInfos = await Service.ImInfoService.GetUserFriendsId(
                new UserInfo() { UserId = AuthenticationTokenStorage.UserId });
            foreach (var user in userInfos)
            {
                var chat = new ChatViewModel(user.UserId, user.IconId, user.DisplayName)
                {
                    LastMessageTime = 0
                };

                ChatListCollection.Add(chat);
            }

            var teamInfos = await Service.ImInfoService.GetUserTeamsId(
                new UserInfo() { UserId = AuthenticationTokenStorage.UserId });
            foreach (var team in teamInfos)
            {
                var chat
                    = new ChatViewModel(team.TeamId, team.IconId, team.TeamName)
                    {
                        LastMessageTime = 0
                    };
                ChatListCollection.Add(chat);
            }
        }

        private void ListBox_SourceUpdated(object sender, EventArgs e)
        {
            ListUpdate();
        }

        private void ListUpdate()
        {
            Decorator decorator = (Decorator)VisualTreeHelper.GetChild(ScrollingListBoxChat, 0);
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
            Initialization.Logger.Debug(currentTargetId);
            var re = await Service.TalkService.SendMessageAsync(new Message()
            {
                Data = ChatInputbox.Text
                ,
                Type = "Text"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = currentTargetId
            });
            Initialization.Logger.Debug(re.Item1.Success);
            if (re.Item1.Success)
            {
                var info = new ChatInfoModel
                {
                    Message = ChatInputbox.Text
                    ,
                    SenderId
                        = AuthenticationTokenStorage.UserId.ToString()
                    ,
                    Type = ChatInfoType.TextTypeChat
                    ,
                    Role = TypeLocalMessageLocation.chatSend
                    ,
                    BImage = MyHeadIcon
                    ,
                    MessageOnwer = AuthenticationTokenStorage.DisplayName
                    ,
                    MessageId = re.Item1.MessageId
                };
                ChatInfos.Add(info);
                currentChatViewModel.LastMessageTime = re.Item1.MessageId;
                currentChatViewModel.LastMessage = ChatInputbox.Text;
                CollectionView.View.Refresh();
                currentChatViewModel.InputData = "";
                ChatInputbox.Text = "";
            }
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
                ScrollingListBoxChat.ItemsSource = chatView.ChatInfos;
                ChatInfos = chatView.ChatInfos;
                currentTargetId = chatView.TargetId;
                ChatInputbox.Text = chatView.InputData;
                currentChatViewModel = chatView;
                currentChatViewModel.NoReadNumber = 0;
            }

            ListUpdate();
        }

        private void ChatInputbox_OnTextChanged(object sender
            , TextChangedEventArgs e)
        {
            currentChatViewModel.LastMessageTime = IdManagement.TimeGen() << 20;
            currentChatViewModel.InputData = ChatInputbox.Text;
            CollectionView.View.Refresh();
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
                            = await Initialization.DataDb.GetBitmapImage(
                                re.MessageId);
                        var info = new ChatInfoModel
                        {
                            Message = image
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
                            MessageOnwer
                                = AuthenticationTokenStorage.DisplayName
                            ,
                            MessageId = re.MessageId
                        };
                        ChatInfos.Add(info);
                        currentChatViewModel.LastMessageTime
                            = IdManagement.TimeGen() << 20;
                        CollectionView.View.Refresh();
                    }
                }
                else
                {
                    MessageBoxX.Show("文件不存在！");
                }
            }
        }

        private async void EventSetter_OnHandler(object sender
            , MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                ListBoxItem temp = sender as ListBoxItem;
                ChatInfoModel chatInfoModel
                    = temp?.DataContext as ChatInfoModel;
                if (chatInfoModel?.Type == ChatInfoType.ImageTypeChat)
                {
                    ImageBrowserWindow window = new ImageBrowserWindow();
                    window.Show();
                    window.Owner = this;
                    window.InitWindow(chatInfoModel.Message as BitmapImage
                        , "查看图片");
                }
            }
            else
            {
                ListBoxItem temp = sender as ListBoxItem;
                ChatInfoModel chatInfoModel
                    = temp?.DataContext as ChatInfoModel;
                if (chatInfoModel?.Type == ChatInfoType.FileTypeChat)
                {
                    var dialog = new SaveFileDialog();
                    dialog.FileName = (string)chatInfoModel.Message;
                    if (dialog.ShowDialog() == true)
                    {
                        var fileName = dialog.FileName;
                        var re = await Initialization.DataDb.GetFileByServer(
                            chatInfoModel.MessageId, fileName);
                        if (re.Success)
                        {
                            Growl.Success("文件保存成功！");
                        }
                    }
                }
            }
        }

        private async void UploadFileButton_OnClick(object sender
            , RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (File.Exists(fileName))
                {
                    var re = await Initialization.DataDb.SendFileToServer(
                        currentTargetId, fileName);
                    if (re.Success)
                    {
                        var info = new ChatInfoModel
                        {
                            Message = Path.GetFileName(fileName)
                            ,
                            SenderId
                                = AuthenticationTokenStorage.UserId
                                    .ToString()
                            ,
                            Type = ChatInfoType.FileTypeChat
                            ,
                            Role = TypeLocalMessageLocation.chatSend
                            ,
                            BImage = MyHeadIcon
                            ,
                            MessageOnwer
                                = AuthenticationTokenStorage.DisplayName
                            ,
                            MessageId = re.MessageId
                        };
                        ChatInfos.Add(info);
                        currentChatViewModel.LastMessageTime
                            = IdManagement.TimeGen() << 20;
                        CollectionView.View.Refresh();
                    }
                }
            }
            else
            {
                MessageBoxX.Show("文件不存在！");
            }
        }
    }
}