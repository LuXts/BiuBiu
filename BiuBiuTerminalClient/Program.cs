using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using Grpc.Net.Client;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BiuBiuTerminalClient
{
    internal class Program
    {
        private static async Task UploadFile(ITalkService clientTalkService
            , string path)
        {
            ulong id = IdManagement.GenerateId(IdType.IconId);
            Console.WriteLine(id);
            var n2 = clientTalkService.SendDataAsync(
                new MessageResponse()
                {
                    MessageId = id
                    ,
                    Type = "Icon"
                    ,
                    Data = path
                    ,
                    SourceId = 0
                    ,
                    TargetId = 0
                }, 55400, true);
            FileStream fs = File.Open(path, FileMode.Open);
            byte[] fileBytes = new byte[fs.Length];
            using (fs)
            {
                //将图片byte信息读入byte数组中
                fs.Read(fileBytes, 0, fileBytes.Length);
                fs.Close();
            }

            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();
            client.Connect(address, 55400);
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

            bool n = (await n2).Success;
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

            bool n = (await n2).Success;
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
             data = IdType.TeamId;
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

            var httpClientHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpClientHandler.ServerCertificateCustomValidationCallback
                = HttpClientHandler
                    .DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);

            var channel2 = GrpcChannel.ForAddress("https://127.0.0.1:5001"
                , new GrpcChannelOptions { HttpClient = httpClient });

            var client2
                = MagicOnion.Client.MagicOnionClient.Create<ITalkService>(
                    channel2
                    , new[]
                    {
                        new AdminWithAuthenticationFilter("18578967136"
                            , "123456789", channel2)
                    });
            await UploadFile(client2, "E://Image/01.jpg");
            await UploadFile(client2, "E://Image/02.jpg");
            await UploadFile(client2, "E://Image/03.jpg");
            await UploadFile(client2, "E://Image/04.jpg");
            await UploadFile(client2, "E://Image/05.jpg");
            await UploadFile(client2, "E://Image/06.jpg");
            await UploadFile(client2, "E://Image/07.jpg");
            await UploadFile(client2, "E://Image/08.jpg");
            await UploadFile(client2, "E://Image/09.jpg");
            await UploadFile(client2, "E://Image/10.jpg");
            await UploadFile(client2, "E://Image/11.jpg");
            await UploadFile(client2, "E://Image/12.jpg");
            await UploadFile(client2, "E://Image/13.jpg");
            await UploadFile(client2, "E://Image/14.jpg");
            await UploadFile(client2, "E://Image/15.jpg");
            await UploadFile(client2, "E://Image/16.jpg");
        }
    }
}