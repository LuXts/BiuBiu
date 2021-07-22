using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.TeamHub;
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

        private TeamHubClient _clientTeamHub;

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

        private string _status = "";

        public string DisplayName
        {
            get { return _displayName + _status; }
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
                Notify("LastMessageTimeStr");
            }
        }

        public string LastMessageTimeStr
        {
            get { return IdManagement.GenerateStrById(LastMessageTime); }
            set { }
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

        private void SetOnlineStatus(bool status)
        {
            if (status)
            {
                _status = "[在线]";
            }
            else
            {
                _status = "[离线]";
            }
            Notify("DisplayName");
        }

        public async Task InitChat(ulong iconId)
        {
            BImage = await Initialization.DataDb.GetBitmapImage(iconId);
            LastMessage = "";

            ulong time = IdManagement.TimeGen();
            ulong oldTime = time - 3600 * 1000 * 24;
            List<Message> reList;
            if (IdManagement.GenerateIdTypeById(TargetId) == IdType.TeamId)
            {
                reList = await Service.TalkService.GetTeamMessagesRecordAsync(
                    TargetId, oldTime << 20, time << 20);
            }
            else
            {
                if (Initialization.OnlineHub.OnlineUserDictionary.TryGetValue(
                    TargetId, out var temp))
                {
                    SetOnlineStatus(true);
                }
                else
                {
                    SetOnlineStatus(false);
                }

                Initialization.OnlineHub.OJEvent += (UserInfo user) =>
                 {
                     if (user.UserId == TargetId)
                     {
                         SetOnlineStatus(true);
                     }
                 };

                Initialization.OnlineHub.OLEvent += (UserInfo user) =>
                {
                    if (user.UserId == TargetId)
                    {
                        SetOnlineStatus(false);
                    }
                };

                reList = await Service.TalkService.GetChatMessagesRecordAsync(
                    _chatId, AuthenticationTokenStorage.UserId, oldTime << 20
                    , time << 20);
            }

            foreach (var message in reList)
            {
                var temp = await MessageToChatInfo.TransformChatInfoModel(message);
                ChatInfos.Add(temp);
                LastMessageTime = message.MessageId;
                if (message.Type == "Text")
                {
                    LastMessage = message.Data;
                }
                else if (message.Type == "Image")
                {
                    LastMessage = "[图片]";
                }
                else
                {
                    LastMessage = "[文件]";
                }
            }
            MainWindow.CollectionView.View.Refresh();
            if (IdManagement.GenerateIdTypeById(TargetId) == IdType.TeamId)
            {
                _clientTeamHub = new TeamHubClient();
                await _clientTeamHub.ConnectAsync(Initialization.GChannel
                    , TargetId);
                _clientTeamHub.SMEvent += async (Message message) =>
                {
                    if (message.SourceId != AuthenticationTokenStorage.UserId)
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

                        var user
                            = await Initialization.DataDb.GetUserInfoByServer(
                                message.SourceId);

                        temp.BImage = await Initialization.DataDb.GetBitmapImage(user.IconId);
                        temp.MessageOnwer = user.DisplayName;
                        this.ChatInfos.Add(temp);
                        this.LastMessage = tip;
                        this.LastMessageTime
                            = IdManagement.TimeGen() << 20;
                        MainWindow.CollectionView.View.Refresh();
                    }
                };
            }
        }
    }
}