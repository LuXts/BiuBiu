using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BiuBiuShare;
using BiuBiuShare.ImInfos;
using BiuBiuShare.SignIn;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tests;
using Grpc.Core;
using Grpc.Net.Client;
using LitJWT;
using LitJWT.Algorithms;
using MagicOnion.Client;
using Microsoft.VisualBasic;

namespace BiuBiuTerminalClient
{
    internal class Program
    {
        private static async Task UploadFile(ITalkService clientTalkService)
        {
            var n2 = clientTalkService.SendDataAsync(
                new MessageResponse() { MessageId = 20191122 }, 34567, true);
            FileStream fs
                = File.Open("F://MyPicture/Snipaste_2021-06-01_07-28-25.png"
                    , FileMode.Open);
            byte[] fileBytes = new byte[fs.Length];
            using (fs)
            {
                //将图片byte信息读入byte数组中
                fs.Read(fileBytes, 0, fileBytes.Length);
                fs.Close();
            }

            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();
            client.Connect(address, 34567);
            using (client)
            {
                //连接完服务器后便在客户端和服务端之间产生一个流的通道
                NetworkStream ns = client.GetStream();
                using (ns)
                {
                    //通过此通道将图片数据写入网络流，传向服务器端接收
                    ns.Write(fileBytes, 0, fileBytes.Length);
                }
            }

            client.Close();

            bool n = await n2;
            Console.WriteLine(n);
        }

        private static async Task DownloadFile(ITalkService clientTalkService)
        {
            int bufferlength = 200;
            byte[] buffer = new byte[bufferlength];

            var n2 = clientTalkService.GetDataAsync(
                new MessageResponse() { MessageId = 20191122 }, 34567, true);
            FileStream fs = File.Open("F://MyPicture/Test.png"
                , FileMode.Create);

            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();
            client.Connect(address, 34567);
            using (client)
            {
                //连接完服务器后便在客户端和服务端之间产生一个流的通道
                NetworkStream ns = client.GetStream();
                Thread.Sleep(500);
                if (ns.DataAvailable)
                {
                    int readLength;
                    //同步读取网络流中的byte信息
                    do
                    {
                        readLength = ns.Read(buffer, 0, bufferlength);
                        Console.WriteLine(readLength);
                        fs.Write(buffer, 0, readLength);
                    } while (readLength > 0);
                }

                fs.Close();
                ns.Close();
                client.Close();
            }

            bool n = await n2;
            Console.WriteLine(n);
        }

        private static async Task Main(string[] args)
        {
            //Id管理类测试
            /*
             IdType data = IdType.UserId;
             ulong id1 = IdManagement.GenerateId(data);
             data = IdType.ModifyId;
             ulong id2=IdManagement.GenerateId(data);
             data = IdType.ChatRecordId;
             ulong id3 = IdManagement.GenerateId(data);
             data = IdType.GroupId;
             ulong id4 = IdManagement.GenerateId(data);
             long timeId = 0;
             int typeId = 0;
             int indexId = 0;
             Console.WriteLine(Convert.ToString(id1));
             Console.WriteLine(Convert.ToString(id2));
             Console.WriteLine(Convert.ToString(id3));
             Console.WriteLine(Convert.ToString(id4));
             Console.WriteLine(id1.ToString("x"));
             Console.WriteLine(id2.ToString("x"));
             Console.WriteLine(id3.ToString("x"));
             Console.WriteLine(id4.ToString("x"));
             Console.WriteLine(IdManagement.GenerateIdByTs((ulong)DateTime.UtcNow.Millisecond).ToString("x"));

             Console.WriteLine(IdManagement.GenerateIdTypeById(id1));
             Console.WriteLine(IdManagement.GenerateIdTypeById(id2));
             Console.WriteLine(IdManagement.GenerateIdTypeById(id3));
             Console.WriteLine(IdManagement.GenerateIdTypeById(id4));
             Console.WriteLine(IdManagement.GenerateTsById(id1).ToString("x"));
             Console.WriteLine(IdManagement.GenerateTsById(id2).ToString("x"));
             Console.WriteLine(IdManagement.GenerateTsById(id3).ToString("x"));
             Console.WriteLine(IdManagement.GenerateTsById(id4).ToString("x"));
            */

            var options = new[]
            {
                // send keepalive ping every 10 second, default is 2 hours
                new ChannelOption("grpc.keepalive_time_ms", 10000),
                // keepalive ping time out after 5 seconds, default is 20 seconds
                new ChannelOption("grpc.keepalive_timeout_ms", 5000)
                ,
                // allow grpc pings from client every 10 seconds
                new ChannelOption("grpc.http2.min_time_between_pings_ms", 10000)
                ,
                // allow unlimited amount of keepalive pings without data
                new ChannelOption("grpc.http2.max_pings_without_data", 0)
                ,
                // allow keepalive pings when there's no gRPC calls
                new ChannelOption("grpc.keepalive_permit_without_calls", 1)
                ,
                // allow grpc pings from client without data every 5 seconds
                new ChannelOption("grpc.http2.min_ping_interval_without_data_ms"
                    , 5000)
                ,
            };

            var channel2 = GrpcChannel.ForAddress("https://localhost:5001");

            var client2
                = MagicOnion.Client.MagicOnionClient.Create<ITalkService>(
                    channel2
                    , new[]
                    {
                        new AdminWithAuthenticationFilter("18578967136"
                            , "123456789", channel2)
                    });
            //await UploadFile(client2);
            //await DownloadFile(client2);
            OnlineHubClient client = new OnlineHubClient();
            client.ConnectAsync(channel2
                , new UserInfo() { UserId = 2019, DisplayName = "Wang" });

            Console.ReadKey();
            OnlineHubClient client1 = new OnlineHubClient();
            client1.ConnectAsync(channel2
                , new UserInfo() { UserId = 2021, DisplayName = "Hu" });
            Console.ReadKey();

            var c = MagicOnionClient.Create<IMyTestService>(channel2);
            int d = await c.SumAsync2(2, 3);

            Console.ReadKey();
            client.DisposeAsync();
            client.WaitForDisconnect();
            client1.DisposeAsync();
            client1.WaitForDisconnect();
        }
    }
}