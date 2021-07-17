using System;
using System.IO;
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
            int bufferlength = 200;
            byte[] buffer = new byte[bufferlength];
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("等待连接");
            //线程会挂在这里，直到客户端发来连接请求
            var client = listener.AcceptTcpClient();
            Console.WriteLine("已经连接");
            //得到从客户端传来的网络流
            var ns = client.GetStream();
            var stdout = File.Open("d://test.png", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            //如果网络流中有数据
            bool temp = false;
            if (ns.DataAvailable)
            {
                int readLength;
                //同步读取网络流中的byte信息
                do
                {
                    readLength = ns.Read(buffer, 0, bufferlength);
                    Console.WriteLine(readLength);
                    stdout.Write(buffer, 0, readLength);
                } while (readLength > 0);

                temp = true;
            }
            stdout.Close();
            ns.Close();
            client.Close();
            listener.Stop();
            return temp;
        }

        public async UnaryResult<bool> GetDataMessage(MessageResponse messageId, int port)
        {
            throw new System.NotImplementedException();
        }
    }
}