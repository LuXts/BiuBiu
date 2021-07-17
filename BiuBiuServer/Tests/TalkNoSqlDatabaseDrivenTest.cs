using System;
using System.Net;
using System.Net.Sockets;
using BiuBiuServer.Interfaces;
using BiuBiuShare;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class TalkNoSqlDatabaseDrivenTest : ITalkNoSqlDatabaseDriven

    {
        private static int bufferlength = 200;
        private static byte[] buffer = new byte[bufferlength];

        public async UnaryResult<bool> AddMessageAsync(MessageResponse message)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<MessageResponse> GetMessagesAsync(ulong messageId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> SendDataMessage(MessageResponse message, int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                Console.WriteLine("等待连接");
                //线程会挂在这里，直到客户端发来连接请求
                var client = listener.AcceptTcpClient();
                Console.WriteLine("已经连接");
                //得到从客户端传来的网络流
                var ns = client.GetStream();
                //如果网络流中有数据
                if (ns.DataAvailable)
                {
                    //同步读取网络流中的byte信息
                    // do
                    //  {
                    //  ns.Read(buffer, 0, bufferlength);
                    //} while (readLength > 0);

                    //异步读取网络流中的byte信息
                    ns.BeginRead(buffer, 0, bufferlength, ReadAsyncCallBack, null);
                }
            }
        }

        public async UnaryResult<bool> GetDataMessage(ulong messageId, int port)
        {
            throw new System.NotImplementedException();
        }
    }
}