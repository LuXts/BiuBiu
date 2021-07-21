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
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Tools;
using MagicOnion.Client;

namespace BiuBiuWpfClient.Model
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this
                    , new PropertyChangedEventArgs(propertyName));
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

        private int _noReadNumber;

        public int NoReadNumber
        {
            get { return _noReadNumber; }
            set
            {
                _noReadNumber = value;
                Notify("NoReadNumber");
            }
        }

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; }
            = new ObservableCollection<ChatInfoModel>();

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

        public ChatViewModel(ulong chatId, ulong iconId, string displayName)
        {
            TargetId = chatId;
            _chatId = chatId;
            DisplayName = displayName;
            NoReadNumber = 0;
            InitChat(iconId);
        }

        public async Task InitChat(ulong iconId)
        {
            BImage = await Initialization.DataDb.GetBitmapImage(iconId);
            LastMessage = "";

            ulong time = IdManagement.TimeGen();
            ulong oldTime = time - 3600 * 1000 * 24;

            var reList = await Service.TalkService.GetChatMessagesRecordAsync(_chatId
                , AuthenticationTokenStorage.UserId, oldTime << 20, time << 20);
            Initialization.Logger.Debug(reList.Count);
            foreach (var message in reList)
            {
                var temp = await MessageToChatInfo.TransformChatInfoModel(message);
                ChatInfos.Add(temp);
                LastMessageTime = message.MessageId;
            }
        }
    }
}