using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;

namespace BiuBiuWpfClient.Tools
{
    public class DataDriven
    {
        private Dictionary<ulong, BitmapImage> _bitmapDictionary = new Dictionary<ulong, BitmapImage>();

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
                    IPAddress address = IPAddress.Parse("127.0.0.1");
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
                IPAddress address = IPAddress.Parse("127.0.0.1");
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
    }
}