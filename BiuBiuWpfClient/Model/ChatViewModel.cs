using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuWpfClient.Login;
using MagicOnion.Client;

namespace BiuBiuWpfClient.Model
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ulong TargetId = 0;

        private BitmapImage _bitmap;

        public BitmapImage BImage
        {
            get { return _bitmap; }
            set
            {
                _bitmap = value;
                Notify("BImage");
            }
        }

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();

        private ulong _chatId;

        private string _lastMessage;

        public string LastMessage
        {
            get { return _lastMessage; }
            set
            {
                _lastMessage = value;
                Notify("LastMessage");
            }
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                Notify("DisplayName");
            }
        }

        private ulong _lastMessageTime;

        public ulong LastMessageTime
        {
            get { return _lastMessageTime; }
            set
            {
                _lastMessageTime = value;
                Notify("LastMessageTime");
            }
        }

        public string InputData;

        private ITalkService _talkService;

        public ChatViewModel(ulong userId)
        {
            TargetId = userId;
            _chatId = userId;
            BImage = new BitmapImage();

            InitChat();
        }

        public async Task InitChat()
        {
            IImInfoService service
                = MagicOnionClient.Create<IImInfoService>(Initialization
                    .GChannel, new[]
                {
                    Initialization.ClientFilter
                });
            var userInfo
                = await service.GetUserInfo(new UserInfo() { UserId = _chatId });
            DisplayName = userInfo.DisplayName;
            LastMessage = userInfo.Description;
            BImage = await Initialization.DataDb
                .GetBitmapImage(userInfo.IconId);
        }
    }
}