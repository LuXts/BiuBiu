using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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

        public ulong LastMessageTime;

        private ITalkService _talkService;

        public ChatViewModel(ulong userId)
        {
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
            ITalkService talkService = MagicOnionClient.Create<ITalkService>(Initialization
                .GChannel, new[]
            {
                Initialization.ClientFilter
            });
            var response
                = await talkService.GetMessageAsync(
                    new Message() { MessageId = userInfo.IconId });
            Initialization.Logger.Debug(response.Item1.Success);
            Initialization.Logger.Debug(response.Item2);
            if (response.Item1.Success)
            {
                var b = talkService.GetDataAsync(response.Item1, response.Item2, true);
                IPAddress address = IPAddress.Parse("127.0.0.1");
                TcpClient client = new TcpClient();
                client.Connect(address, (int)response.Item2);
                using (client)
                {
                    //连接完服务器后便在客户端和服务端之间产生一个流的通道
                    NetworkStream ns = client.GetStream();
                    //Thread.Sleep(200);
                    while (!ns.DataAvailable)
                    {
                    }
                    int bufferlength = 2048;
                    byte[] buffer = new byte[bufferlength];
                    var ms = new MemoryStream();
                    int readLength;
                    do
                    {
                        readLength = ns.Read(buffer, 0, bufferlength);
                        ms.Write(buffer, 0, readLength);
                    } while (readLength > 0);
                    Initialization.Logger.Debug(ms.Length);
                    ms.Position = 0;
                    var bitbmp = new BitmapImage();
                    bitbmp.BeginInit();
                    bitbmp.StreamSource = ms;
                    bitbmp.EndInit();
                    bitbmp.Freeze();
                    BImage = bitbmp;
                    ns.Close();
                    client.Close();
                }
            }
        }
    }
}