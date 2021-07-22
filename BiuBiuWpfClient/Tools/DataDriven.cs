using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using MagicOnion;

namespace BiuBiuWpfClient.Tools
{
    public class DataDriven
    {
        private Dictionary<ulong, BitmapImage> _bitmapDictionary = new Dictionary<ulong, BitmapImage>();

        private Dictionary<ulong, UserInfoResponse> _userInfoDictionary = new Dictionary<ulong, UserInfoResponse>();

        private Dictionary<ulong, FriendRequestResponse>
            _friendRequestDictionary
                = new Dictionary<ulong, FriendRequestResponse>();

        private Dictionary<ulong, TeamInvitationResponse>
            _teamInvitationsdDictionary
                = new Dictionary<ulong, TeamInvitationResponse>();

        public async Task<UserInfoResponse> GetUserInfoByServer(ulong userId)
        {
            lock (_userInfoDictionary)
            {
                if (_userInfoDictionary.ContainsKey(userId))
                {
                    return _userInfoDictionary[userId];
                }
            }
            var response
                = await Service.ImInfoService.GetUserInfo(
                    new UserInfo() { UserId = userId });
            if (response.Success)
            {
                lock (_userInfoDictionary)
                {
                    if (!_userInfoDictionary.ContainsKey(userId))
                    {
                        _userInfoDictionary.Add(userId, response);
                    }
                }
            }
            return response;
        }

        public async Task<BitmapImage> GetBitmapImage(ulong imageId)
        {
            lock (_bitmapDictionary)
            {
                if (_bitmapDictionary.ContainsKey(imageId))
                {
                    return _bitmapDictionary[imageId];
                }
            }

            var array = Initialization.LiteDb.LoadImage(imageId);
            while (array.Length == 0)
            {
                var response
                    = await Service.TalkService.GetMessageAsync(
                        new Message() { MessageId = imageId });
                Initialization.Logger.Debug(response.Item1.Success);
                Initialization.Logger.Debug(response.Item2);
                if (response.Item1.Success)
                {
                    var b = Service.TalkService.GetDataAsync(response.Item1
                        , response.Item2, true);
                    IPAddress address = IPAddress.Parse(Initialization.GrpcIp);
                    TcpClient client = new TcpClient();
                    client.Connect(address, (int)response.Item2);
                    using (client)
                    {
                        NetworkStream ns = client.GetStream();
                        while (!ns.DataAvailable)
                        {
                        }

                        Initialization.LiteDb.UploadImage(imageId, ns);
                        ns.Close();
                        client.Close();
                    }
                }
                else
                {
                    return null;
                }

                array = Initialization.LiteDb.LoadImage(imageId);
            }

            var bitbmp = new BitmapImage();
            bitbmp.BeginInit();
            bitbmp.StreamSource = new MemoryStream(array);
            bitbmp.EndInit();
            bitbmp.Freeze();

            lock (_bitmapDictionary)
            {
                if (!_bitmapDictionary.ContainsKey(imageId))
                {
                    _bitmapDictionary.Add(imageId, bitbmp);
                }
            }

            return bitbmp;
        }

        public BitmapImage AddBitmapImage(ulong imageId, byte[] array)
        {
            lock (_bitmapDictionary)
            {
                if (_bitmapDictionary.ContainsKey(imageId))
                {
                    return _bitmapDictionary[imageId];
                }
            }

            var bitbmp = new BitmapImage();
            bitbmp.BeginInit();
            bitbmp.StreamSource = new MemoryStream(array);
            bitbmp.EndInit();
            bitbmp.Freeze();

            lock (_bitmapDictionary)
            {
                if (!_bitmapDictionary.ContainsKey(imageId))
                {
                    _bitmapDictionary.Add(imageId, bitbmp);
                }
            }

            Initialization.LiteDb.UploadImage(imageId, new MemoryStream(array));

            return bitbmp;
        }

        public async Task<MessageResponse> SendImageToServer(ulong targetId, string imagePath)
        {
            var reAsync = await Service.TalkService.SendMessageAsync(
                new Message()
                {
                    Type = "Image"
                    ,
                    Data = Path.GetFileName(imagePath)
                    ,
                    SourceId = AuthenticationTokenStorage.UserId
                    ,
                    TargetId = targetId
                });
            byte[] fileBytes;
            if (reAsync.Item1.Success)
            {
                fileBytes = File.ReadAllBytes(imagePath);
                AddBitmapImage(reAsync.Item1.MessageId, fileBytes);
                var reSendDataAsync = Service.TalkService.SendDataAsync(
                    reAsync.Item1, reAsync.Item2, true);
                IPAddress address = IPAddress.Parse(Initialization.GrpcIp);
                TcpClient client = new TcpClient();
                client.Connect(address, Convert.ToInt32(reAsync.Item2));
                using (client)
                {
                    //连接完服务器后便在客户端和服务端之间产生一个流的通道
                    NetworkStream ns = client.GetStream();
                    using (ns)
                    {
                        //通过此通道将图片数据写入网络流，传向服务器端接收
                        ns.Write(fileBytes, 0, fileBytes.Length);
                        ns.Close();
                    }
                }
                client.Close();
            }

            return reAsync.Item1;
        }

        public async Task<MessageResponse> SendFileToServer(ulong targetId
            , string filePath)
        {
            var reAsync = await Service.TalkService.SendMessageAsync(
                new Message()
                {
                    Type = "File"
                    ,
                    Data = Path.GetFileName(filePath)
                    ,
                    SourceId = AuthenticationTokenStorage.UserId
                    ,
                    TargetId = targetId
                });

            if (reAsync.Item1.Success)
            {
                int bufferlength = 1024 * 1024;
                byte[] buffer = new byte[bufferlength];

                FileStream fs = new FileStream(filePath, FileMode.Open);

                var reSendDataAsync = Service.TalkService.SendDataAsync(
                    reAsync.Item1, reAsync.Item2, true);
                IPAddress address = IPAddress.Parse(Initialization.GrpcIp);
                TcpClient client = new TcpClient();
                client.Connect(address, Convert.ToInt32(reAsync.Item2));
                using (client)
                {
                    //连接完服务器后便在客户端和服务端之间产生一个流的通道
                    NetworkStream ns = client.GetStream();

                    int readLength;
                    //同步读取网络流中的byte信息
                    do
                    {
                        readLength = fs.Read(buffer, 0, bufferlength);
                        ns.Write(buffer, 0, readLength);
                    } while (readLength > 0);
                    ns.Close();
                }

                client.Close();
                fs.Close();
                return await reSendDataAsync;
            }
            return MessageResponse.Failed;
        }

        public async Task<MessageResponse> GetFileByServer(ulong messageId
            , string filePath)
        {
            int bufferlength = 1024 * 1024;
            byte[] buffer = new byte[bufferlength];

            var n2 = await Service.TalkService.GetMessageAsync(
                new MessageResponse() { MessageId = messageId });

            if (n2.Item1.Success)
            {
                var re = Service.TalkService.GetDataAsync(n2.Item1, n2.Item2
                    , true);

                FileStream fs = File.Open(filePath
                    , FileMode.Create);
                IPAddress address = IPAddress.Parse(Initialization.GrpcIp);
                TcpClient client = new TcpClient();
                client.Connect(address, Convert.ToInt32(n2.Item2));
                using (client)
                {
                    NetworkStream ns = client.GetStream();
                    while (!ns.DataAvailable)
                    {
                    }

                    int readLength;
                    do
                    {
                        readLength = ns.Read(buffer, 0, bufferlength);
                        Initialization.Logger.Debug(readLength);
                        fs.Write(buffer, 0, readLength);
                    } while (readLength > 0);

                    fs.Close();
                    ns.Close();
                    client.Close();
                    return await re;
                }
            }
            return MessageResponse.Failed;
        }

        public FriendRequestResponse GetFriendRequest(
            ulong requestId)
        {
            lock (_friendRequestDictionary)
            {
                if (_friendRequestDictionary.ContainsKey(requestId))
                {
                    return _friendRequestDictionary[requestId];
                }
            }
            return FriendRequestResponse.Failed;
        }

        public void StorageFriendRequest(FriendRequest friendRequest)
        {
            lock (_friendRequestDictionary)
            {
                _friendRequestDictionary[friendRequest.RequestId]
                    = new FriendRequestResponse(friendRequest) { Success = true };
            }
        }

        public TeamInvitationResponse GetTeamInvitation(
            ulong invitationId)
        {
            lock (_teamInvitationsdDictionary)
            {
                if (_teamInvitationsdDictionary.ContainsKey(invitationId))
                {
                    return _teamInvitationsdDictionary[invitationId];
                }
            }
            return TeamInvitationResponse.Failed;
        }

        public void StorageTeamInvitation(TeamInvitation teamInvitation)
        {
            lock (_teamInvitationsdDictionary)
            {
                _teamInvitationsdDictionary[teamInvitation.InvitationId]
                    = new TeamInvitationResponse(teamInvitation);
            }
        }
    }
}