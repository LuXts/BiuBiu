using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
using BiuBiuShare;
using BiuBiuShare.Response;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.TeamHub;
using BiuBiuShare.Tool;
using BiuBiuShare.UserHub;
using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    /// <summary>
    /// 聊天相关服务实现
    /// </summary>
    [Authorize]
    public class TalkService : ServiceBase<ITalkService>, ITalkService
    {
        private readonly ITalkNoSqlDatabaseDriven _noSQLDriven
            = new TalkNoSqlDatabaseDriven();

        public static LinkedList<uint> PortList = new LinkedList<uint>();

        /// <inheritdoc />
        public async UnaryResult<(MessageResponse, uint)> SendMessageAsync(
            Message message)
        {
            MessageResponse response;
            var temp = new MessageResponse()
            {
                Data = message.Data
                ,
                MessageId = IdManagement.GenerateId(IdType.MessageId)
                ,
                Success = true
                ,
                SourceId = message.SourceId
                ,
                TargetId = message.TargetId
                ,
                Type = message.Type
            };
            uint port;
            if (message.Type.Equals("Text"))
            {
                port = 0;
                if (await _noSQLDriven.AddMessageAsync(temp))
                {
                    response = temp;
                    await ForwardMessage(response);
                }
                else
                {
                    response = MessageResponse.Failed;
                }
            }
            else
            {
                lock (PortList)
                {
                    port = PortList.First.Value + 55000;
                    PortList.RemoveFirst();
                }
                response = temp;
            }

            return (response, port);
        }

        private static async Task ForwardMessage(MessageResponse response)
        {
            var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
            if (IdManagement.GenerateIdTypeById(response.TargetId) == IdType.UserId)
            {
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel, response.TargetId);
                client.SendMessage(response);
                client.DisposeAsync();
                client.WaitForDisconnect();
            }
            else
            {
                TeamHubClient client = new TeamHubClient();
                await client.ConnectAsync(channel, response.TargetId);
                client.SendMessage(response);
                client.DisposeAsync();
                client.WaitForDisconnect();
            }
        }

        /// <inheritdoc />
        public async UnaryResult<bool> SendDataAsync(MessageResponse message
            , uint port, bool respond)
        {
            if (respond)
            {
                bool re0 = await _noSQLDriven.SendDataMessage(message, port);
                lock (PortList)
                {
                    PortList.AddFirst(port - 55000);
                }
                bool re1 = await _noSQLDriven.AddMessageAsync(message);
                if (re0 && re1)
                {
                    ForwardMessage(message);
                }
                return re0 && re1;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async UnaryResult<(MessageResponse, uint)> GetMessageAsync(
            ulong messageId)
        {
            var response = await _noSQLDriven.GetMessagesAsync(messageId);
            lock (PortList)
            {
                uint port = PortList.First.Value + 55000;
                PortList.RemoveFirst();
                return (response, port);
            }
        }

        /// <inheritdoc />
        public async UnaryResult<bool> GetDataAsync(MessageResponse message
            , uint port, bool respond)
        {
            bool re = await _noSQLDriven.GetDataMessage(message, port);
            lock (PortList)
            {
                PortList.AddFirst(port);
            }
            return re;
        }

        public async UnaryResult<List<MessageResponse>>
            GetNoReadMessageRecordAsync(ulong targetId, ulong startTime
                , ulong endTime)
        {
            return await _noSQLDriven.GetMessagesRecordAsync(targetId, startTime
                , endTime);
        }

        /// <inheritdoc />
        public async UnaryResult<List<MessageResponse>>
            GetChatMessagesRecordAsync(ulong sourceId, ulong targetId
                , ulong startTime, ulong endTime)
        {
            return await _noSQLDriven.GetChatMessagesRecordAsync(sourceId
                , targetId, startTime, endTime);
        }

        /// <inheritdoc />
        public async UnaryResult<List<MessageResponse>>
            GetTeamMessagesRecordAsync(ulong teamId, ulong startTime
                , ulong endTime)
        {
            return await _noSQLDriven.GetMessagesRecordAsync(teamId, startTime
                , endTime);
        }
    }
}