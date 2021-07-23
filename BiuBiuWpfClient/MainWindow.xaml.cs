using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;
using BiuBiuWpfClient.Tools;
using BiuBiuWpfClient.Userhub;
using HandyControl.Controls;
using Microsoft.Win32;
using Panuon.UI.Silver;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ScrollViewer = System.Windows.Controls.ScrollViewer;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this
                    , new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; }
            = new ObservableCollection<ChatInfoModel>();

        public static ObservableCollection<ChatViewModel> ChatListCollection
            = new ObservableCollection<ChatViewModel>();

        public static CollectionViewSource CollectionView
            = new CollectionViewSource();

        private UserHubClient _userHubClient;

        private ulong currentTargetId = 0;

        private ChatViewModel currentChatViewModel;

        private BitmapImage _myIcon;

        public BitmapImage MyHeadIcon
        {
            get { return _myIcon; }
            set
            {
                _myIcon = value;
                Notify("MyHeadIcon");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

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
                var temp
                    = await MessageToChatInfo.TransformChatInfoModel(message);
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

            _userHubClient.SFTEvent += async (FriendRequest request) =>
            {
                if (request.SenderId == AuthenticationTokenStorage.UserId)
                {
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            request.ReceiverId);
                    if (request.RequestResult == "True")
                    {
                        ChatListCollection.Add(new ChatViewModel(
                            request.ReceiverId, user.IconId, user.DisplayName));
                        InfoViewModel.FriendCollection.Add(new InfoListItem()
                        {
                            BImage = await Initialization.DataDb.GetBitmapImage(user.IconId)
                            ,
                            DisplayName = user.DisplayName,
                            InfoId = user.UserId,
                            Type = InfoListItem.InfoType.Friend
                        });
                        Growl.Info(user.DisplayName + "接受了你的好友请求。");
                    }
                    else
                    {
                        Growl.Info(user.DisplayName + "拒绝了你的好友请求。");
                    }
                }
                else
                {
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            request.SenderId);
                    InfoViewModel.NewFriendCollection.Add(new InfoListItem()
                    {
                        BImage
                            = await Initialization.DataDb.GetBitmapImage(
                                user.IconId)
                        ,
                        DisplayName = user.DisplayName
                        ,
                        InfoId = request.RequestId
                        ,
                        Type = InfoListItem.InfoType.NewFriend
                    });
                    Initialization.DataDb.StorageFriendRequest(request);
                    Growl.Info("你有一条新的好友请求。");
                }
            };

            _userHubClient.SGIEvent += async (TeamInvitation invitation) =>
            {
                var team
                    = await Initialization.DataDb.GetTeamInfoByServer(
                        invitation.TeamId);
                if (invitation.ReceiverId != AuthenticationTokenStorage.UserId)
                {
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            invitation.ReceiverId);
                    if (invitation.InvitationResult == "True")
                    {
                        Growl.Info(user.DisplayName + "接受了你的邀请，加入了" + team.TeamName + "群。");
                    }
                    else
                    {
                        Growl.Info(user.DisplayName + "拒绝了你的邀请。");
                    }
                }
                else
                {
                    InfoViewModel.TeamInvitationCollection.Add(new InfoListItem()
                    {
                        BImage
                            = await Initialization.DataDb.GetBitmapImage(
                                team.IconId)
                        ,
                        DisplayName = team.TeamName
                        ,
                        InfoId = invitation.InvitationId
                        ,
                        Type = InfoListItem.InfoType.TeamInvitation
                    });
                    Initialization.DataDb.StorageTeamInvitation(invitation);
                    Growl.Info("你有一条新的群组邀请。");
                }
            };

            _userHubClient.SGREvent += async (TeamRequest request) =>
            {
                var user
                    = await Initialization.DataDb.GetUserInfoByServer(
                        request.SenderId);
                var team
                    = await Initialization.DataDb.GetTeamInfoByServer(request
                        .TeamId);
                if (request.SenderId == AuthenticationTokenStorage.UserId)
                {
                    if (request.RequestResult == "True")
                    {
                        ChatListCollection.Add(new ChatViewModel(
                            team.TeamId, team.IconId, team.TeamName));
                        InfoViewModel.TeamCollection.Add(new InfoListItem()
                        {
                            BImage = await Initialization.DataDb.GetBitmapImage(team.IconId)
                            ,
                            DisplayName = team.TeamName,
                            InfoId = team.TeamId,
                            Type = InfoListItem.InfoType.Team
                        });
                        Growl.Info(team.TeamName + "的群主同意了你的申请，你现在已经成功加入群聊！");
                    }
                    else
                    {
                        Growl.Info(team.TeamName + "的群主拒绝了你的申请。");
                    }
                }
                else
                {
                    InfoViewModel.TeamRequestCollection.Add(new InfoListItem()
                    {
                        BImage
                            = await Initialization.DataDb.GetBitmapImage(
                                user.IconId)
                        ,
                        DisplayName = user.DisplayName
                        ,
                        InfoId = request.RequestId
                        ,
                        Type = InfoListItem.InfoType.TeamRequest
                    });
                    Initialization.DataDb.StorageTeamRequestd(request);
                    Growl.Info("你的" + team.TeamName + "群组有一条新的入群申请。");
                }
            };

            this.Closed += MainWindow_Closed;

            var temp = WindowXCaption.GetHeaderTemplate(this);
            Initialization.Logger.Debug(temp.GetType().Name);
            Initialization.Logger.Debug(temp.Template.GetType().Name);
            Initialization.Logger.Debug(temp.Triggers);
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
                InfoViewModel.FriendCollection.Add(new InfoListItem()
                {
                    InfoId = user.UserId
                    ,
                    DisplayName = user.DisplayName
                    ,
                    BImage
                        = await Initialization.DataDb.GetBitmapImage(
                            user.IconId)
                    ,
                    Type = InfoListItem.InfoType.Friend
                });
                var chat
                    = new ChatViewModel(user.UserId, user.IconId
                        , user.DisplayName)
                    { LastMessageTime = 0 };

                ChatListCollection.Add(chat);
            }

            var teamInfos = await Service.ImInfoService.GetUserTeamsId(
                new UserInfo() { UserId = AuthenticationTokenStorage.UserId });
            foreach (var team in teamInfos)
            {
                InfoViewModel.TeamCollection.Add(new InfoListItem()
                {
                    InfoId = team.TeamId
                    ,
                    DisplayName = team.TeamName
                    ,
                    BImage
                        = await Initialization.DataDb.GetBitmapImage(
                            team.IconId)
                    ,
                    Type = InfoListItem.InfoType.Team
                });
                var chat
                    = new ChatViewModel(team.TeamId, team.IconId, team.TeamName)
                    {
                        LastMessageTime = 0
                    };
                ChatListCollection.Add(chat);
            }

            InitInfo();
        }

        private async void InitInfo()
        {
            var friendRequest
                = await Service.GroFriService.GetFriendRequest(
                    AuthenticationTokenStorage.UserId);
            foreach (var request in friendRequest)
            {
                Initialization.DataDb.StorageFriendRequest(request);

                var user
                    = await Initialization.DataDb.GetUserInfoByServer(
                        request.SenderId);

                InfoViewModel.NewFriendCollection.Add(new InfoListItem()
                {
                    BImage
                        = await Initialization.DataDb.GetBitmapImage(
                            user.IconId)
                    ,
                    DisplayName = user.DisplayName
                    ,
                    InfoId = request.RequestId
                    ,
                    Type = InfoListItem.InfoType.NewFriend
                });
            }

            var teamInvitation
                = await Service.GroFriService.GetGroupInvitation(
                    AuthenticationTokenStorage.UserId);
            foreach (var invitation in teamInvitation)
            {
                Initialization.DataDb.StorageTeamInvitation(invitation);

                var team
                    = await Initialization.DataDb.GetTeamInfoByServer(invitation
                        .TeamId);

                InfoViewModel.TeamInvitationCollection.Add(new InfoListItem()
                {
                    BImage
                        = await Initialization.DataDb.GetBitmapImage(
                            team.IconId)
                    ,
                    DisplayName = team.TeamName
                    ,
                    InfoId = invitation.InvitationId
                    ,
                    Type = InfoListItem.InfoType.TeamInvitation
                });
            }

            var teamRequest
                = await Service.GroFriService.GetGroupRequest(
                    AuthenticationTokenStorage.UserId);
            foreach (var request in teamRequest)
            {
                Initialization.DataDb.StorageTeamRequestd(request);

                var user
                    = await Initialization.DataDb.GetUserInfoByServer(
                        request.SenderId);

                InfoViewModel.TeamRequestCollection.Add(new InfoListItem()
                {
                    BImage
                        = await Initialization.DataDb.GetBitmapImage(
                            user.IconId)
                    ,
                    DisplayName = user.DisplayName
                    ,
                    InfoId = request.RequestId
                    ,
                    Type = InfoListItem.InfoType.TeamRequest
                });
            }
        }

        private void ListBox_SourceUpdated(object sender, EventArgs e)
        {
            ListUpdate();
        }

        private void ListUpdate()
        {
            Decorator decorator
                = (Decorator)VisualTreeHelper.GetChild(ScrollingListBoxChat
                    , 0);
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
            if (ChatInputbox.Text == "")
            {
                Growl.Warning("不能发送空消息！");
                return;
            }
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

            ListUpdate();
        }

        private void ChatListBox_OnSelectionChanged(object sender
            , SelectionChangedEventArgs e)
        {
            if (ChatView.Visibility == Visibility.Hidden)
            {
                ChatView.Visibility = Visibility.Visible;
            }

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
                        ListUpdate();
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
                    window.InitWindow(chatInfoModel.Message as BitmapImage, "");
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
                var re = await Initialization.DataDb.SendFileToServer(
                    currentTargetId, fileName);
                if (re.Success)
                {
                    var info = new ChatInfoModel
                    {
                        Message = Path.GetFileName(fileName)
                        ,
                        SenderId
                            = AuthenticationTokenStorage.UserId.ToString()
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
                    ListUpdate();
                }
            }
        }

        private bool _talkSwicth = true;

        private void TalkSwitchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_talkSwicth != true)
            {
                SolidColorBrush myBrush1
                    = new SolidColorBrush(
                        Color.FromArgb(0xFF, 0xD1, 0xD3, 0xD5));
                TalkSwitch.Background = (System.Windows.Media.Brush)myBrush1;
                _talkSwicth = true;
                SolidColorBrush myBrush2
                    = new SolidColorBrush(
                        Color.FromArgb(0xFF, 0xEB, 0xEB, 0xEB));
                AddressSwitch.Background
                    = (System.Windows.Media.Brush)myBrush2;
                _addressSwitch = false;
                ChatListBox.Visibility = Visibility.Visible;
                ChatView.Visibility = _visibility;
                AddressPanel.Visibility = Visibility.Collapsed;
                _visibility2 = InfoPanel.Visibility;
                InfoPanel.Visibility = Visibility.Collapsed;
            }
        }

        private bool _addressSwitch = false;

        private Visibility _visibility = Visibility.Collapsed;

        private Visibility _visibility2 = Visibility.Collapsed;

        public void AddressBookSwitchButton_OnClick(object sender
            , RoutedEventArgs e)
        {
            if (_addressSwitch != true)
            {
                SolidColorBrush myBrush1
                    = new SolidColorBrush(
                        Color.FromArgb(0xFF, 0xEB, 0xEB, 0xEB));
                TalkSwitch.Background = (System.Windows.Media.Brush)myBrush1;
                _talkSwicth = false;
                SolidColorBrush myBrush2
                    = new SolidColorBrush(
                        Color.FromArgb(0xFF, 0xD1, 0xD3, 0xD5));
                AddressSwitch.Background
                    = (System.Windows.Media.Brush)myBrush2;
                _addressSwitch = true;
                _visibility = ChatView.Visibility;
                ChatListBox.Visibility = Visibility.Collapsed;
                ChatView.Visibility = Visibility.Collapsed;
                AddressPanel.Visibility = Visibility.Visible;
                InfoPanel.Visibility = _visibility2;
            }
        }

        private readonly static SolidColorBrush _noSelectBrush
            = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

        private readonly static SolidColorBrush _selectBrush
            = new SolidColorBrush(Color.FromArgb(0xFF, 0xEF, 0xF0, 0xF1));

        private void AFriendsButton_OnClick(object sender, RoutedEventArgs e)
        {
            InfoPanel.Visibility = Visibility.Visible;
            InfoTitle.Text = "好友";
            InfoListBox.ItemsSource = InfoViewModel.FriendCollection;
            AFriendsButton.Background = _selectBrush;
            ANewFriendsButton.Background = _noSelectBrush;
            ATeamInvitationButton.Background = _noSelectBrush;
            ATeamRequestButton.Background = _noSelectBrush;
            ATeamsButton.Background = _noSelectBrush;
        }

        private void ANewFriendsButton_OnClick(object sender, RoutedEventArgs e)
        {
            InfoPanel.Visibility = Visibility.Visible;
            InfoTitle.Text = "新的好友";
            InfoListBox.ItemsSource = InfoViewModel.NewFriendCollection;
            AFriendsButton.Background = _noSelectBrush;
            ANewFriendsButton.Background = _selectBrush;
            ATeamInvitationButton.Background = _noSelectBrush;
            ATeamRequestButton.Background = _noSelectBrush;
            ATeamsButton.Background = _noSelectBrush;
        }

        private void ATeamsButton_OnClick(object sender, RoutedEventArgs e)
        {
            InfoPanel.Visibility = Visibility.Visible;
            InfoTitle.Text = "我的群组";
            InfoListBox.ItemsSource = InfoViewModel.TeamCollection;
            AFriendsButton.Background = _noSelectBrush;
            ANewFriendsButton.Background = _noSelectBrush;
            ATeamInvitationButton.Background = _noSelectBrush;
            ATeamRequestButton.Background = _noSelectBrush;
            ATeamsButton.Background = _selectBrush;
        }

        private void ATeamInvitationButton_OnClick(object sender
            , RoutedEventArgs e)
        {
            InfoPanel.Visibility = Visibility.Visible;
            InfoTitle.Text = "群组邀请";
            InfoListBox.ItemsSource = InfoViewModel.TeamInvitationCollection;
            AFriendsButton.Background = _noSelectBrush;
            ANewFriendsButton.Background = _noSelectBrush;
            ATeamInvitationButton.Background = _selectBrush;
            ATeamRequestButton.Background = _noSelectBrush;
            ATeamsButton.Background = _noSelectBrush;
        }

        private void ATeamRequestButton_OnClick(object sender
            , RoutedEventArgs e)
        {
            InfoPanel.Visibility = Visibility.Visible;
            InfoTitle.Text = "入群审核";
            InfoListBox.ItemsSource = InfoViewModel.TeamRequestCollection;
            AFriendsButton.Background = _noSelectBrush;
            ANewFriendsButton.Background = _noSelectBrush;
            ATeamInvitationButton.Background = _noSelectBrush;
            ATeamRequestButton.Background = _selectBrush;
            ATeamsButton.Background = _noSelectBrush;
        }

        private async void VideoButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IdManagement.GenerateIdTypeById(currentTargetId) ==
                IdType.UserId)
            {
                if (currentChatViewModel.Status == "[在线]")
                {
                    MessageBoxX.Show("视频功能未完成！");
                }
                else
                {
                    MessageBoxX.Show("对方不在线！");
                }
            }
            else
            {
                MessageBoxX.Show("群组暂时无法视频聊天！");
            }
        }

        private UserInfoWindow uWindow;

        private TeamInfoWindow tWindow;

        private NotificationWindow nWindow;

        private async void InfoListBox_OnSelectionChanged(object sender
            , SelectionChangedEventArgs e)
        {
            var infoListItem = InfoListBox.SelectedItem as InfoListItem;
            if (infoListItem is null)
            {
            }
            else
            {
                if (!(uWindow is null))
                {
                    uWindow.Close();
                }

                if (!(tWindow is null))
                {
                    tWindow.Close();
                }

                if (!(nWindow is null))
                {
                    nWindow.Close();
                }

                if (infoListItem.Type == InfoListItem.InfoType.Friend)
                {
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            infoListItem.InfoId);
                    uWindow = new UserInfoWindow();

                    uWindow.InitInfo(user
                        , await Initialization.DataDb.GetBitmapImage(
                            user.IconId));
                    uWindow.Show();
                }
                else if (infoListItem.Type == InfoListItem.InfoType.Team)
                {
                    var team
                        = await Initialization.DataDb.GetTeamInfoByServer(
                            infoListItem.InfoId);
                    tWindow = new TeamInfoWindow();
                    tWindow.InitInfo(team
                        , await Initialization.DataDb.GetBitmapImage(
                            team.IconId));
                    tWindow.Show();
                }
                else if (infoListItem.Type == InfoListItem.InfoType.NewFriend)
                {
                    var item
                        = Initialization.DataDb.GetFriendRequest(infoListItem
                            .InfoId);
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            item.SenderId);
                    nWindow = new NotificationWindow();
                    nWindow.Init(user.DisplayName, item.RequestMessage
                        , await Initialization.DataDb.GetBitmapImage(
                            user.IconId), infoListItem.InfoId);
                    nWindow.Show();
                }
                else if (infoListItem.Type ==
                         InfoListItem.InfoType.TeamInvitation)
                {
                    var item
                        = Initialization.DataDb.GetTeamInvitation(infoListItem
                            .InfoId);
                    var team
                        = await Initialization.DataDb.GetTeamInfoByServer(
                            item.TeamId);
                    nWindow = new NotificationWindow();
                    nWindow.Init(team.TeamName, item.InvitationMessage
                        , await Initialization.DataDb.GetBitmapImage(
                            team.IconId), infoListItem.InfoId);
                    nWindow.Show();
                }
                else if (infoListItem.Type == InfoListItem.InfoType.TeamRequest)
                {
                    var item
                        = Initialization.DataDb.GetTeamRequest(infoListItem
                            .InfoId);
                    var user
                        = await Initialization.DataDb.GetUserInfoByServer(
                            item.SenderId);
                    nWindow = new NotificationWindow();
                    nWindow.Init(user.DisplayName, item.RequestMessage
                        , await Initialization.DataDb.GetBitmapImage(
                            user.IconId), infoListItem.InfoId);
                    nWindow.Show();
                }

                InfoListBox.SelectionMode = SelectionMode.Multiple;
                InfoListBox.SelectedItems.Remove(infoListItem);
                InfoListBox.SelectionMode = SelectionMode.Single;
            }
        }

        private async void HeadButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(uWindow is null))
            {
                uWindow.Close();
            }

            if (!(tWindow is null))
            {
                tWindow.Close();
            }

            if (!(nWindow is null))
            {
                nWindow.Close();
            }

            var user
                = await Initialization.DataDb.GetUserInfoByServer(
                    AuthenticationTokenStorage.UserId);
            uWindow = new UserInfoWindow();
            uWindow.InitInfo(user, this.MyHeadIcon);
            uWindow.Show();
        }

        private void AddUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AddFOrGWindow();
            window.ShowDialog();
        }

        private void ANewTeamButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new NewTeamWindow();
            window.ShowDialog();
        }

        private void SettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new EditPasswordWindow();
            window.ShowDialog();
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ChatView.Visibility == Visibility.Visible)
            {
                SendMessageButton_OnClick(sender, e);
            }
        }
    }
}