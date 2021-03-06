using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuServer.TeamHub;
using BiuBiuServer.UserHub;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using MagicOnion;
using MagicOnion.Server;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiuBiuServer.Services
{
    /// <summary>
    /// 聊天相关服务实现
    /// </summary>
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
            var temp = new Message()
            {
                Data = message.Data
                ,
                MessageId = IdManagement.GenerateId(IdType.MessageId)
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
                if ((await _noSQLDriven.AddMessageAsync(temp)).Success)
                {
                    response = new MessageResponse(temp) { Success = true };
                    await ForwardMessage(message);
                }
                else
                {
                    response = MessageResponse.Failed;
                }
            }
            else if (message.Type.Equals("Video"))
            {
                await ForwardMessage(message);
                port = 0;
                response = new MessageResponse() { Success = true };
            }
            else
            {
                lock (PortList)
                {
                    port = PortList.First.Value + 55000;
                    PortList.RemoveFirst();
                }
                response = new MessageResponse(temp) { Success = true };
            }
            return (response, port);
        }

        private static async Task ForwardMessage(Message response)
        {
            if (IdManagement.GenerateIdTypeById(response.TargetId) ==
                IdType.UserId)
            {
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(Initialization.GChannel, response.TargetId);
                client.ServerSendMessage(response);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            else
            {
                TeamHubClient client = new TeamHubClient();
                await client.ConnectAsync(Initialization.GChannel, response.TargetId);
                client.ServerSendMessage(response);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
        }

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> SendDataAsync(Message message
            , uint port, bool respond)
        {
            if (respond)
            {
                var re0 = await _noSQLDriven.SendDataMessage(message, port);
                lock (PortList)
                {
                    PortList.AddFirst(port - 55000);
                }

                var re1 = await _noSQLDriven.AddMessageAsync(message);
                if (re0.Success && re1.Success)
                {
                    await ForwardMessage(message);
                    return re1;
                }
                else
                {
                    return MessageResponse.Failed;
                }
            }
            else
            {
                return MessageResponse.Failed;
            }
        }

        /// <inheritdoc />
        public async UnaryResult<(MessageResponse, uint)> GetMessageAsync(
            Message message)
        {
            var response = await _noSQLDriven.GetMessagesAsync(message);
            if (response.Success)
            {
                uint port;
                lock (PortList)
                {
                    port = PortList.First.Value + 55000;
                    PortList.RemoveFirst();
                }
                return (response, port);
            }
            else
            {
                return (response, 0);
            }
        }

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> GetDataAsync(Message message
            , uint port, bool respond)
        {
            var re = await _noSQLDriven.GetDataMessage(message, port);
            lock (PortList)
            {
                PortList.AddLast(port - 55000);
            }

            return re;
        }

        public async UnaryResult<List<Message>>
            GetNoReadMessageRecordAsync(ulong targetId, ulong startTime
                , ulong endTime)
        {
            return await _noSQLDriven.GetMessagesRecordAsync(targetId, startTime
                , endTime);
        }

        /// <inheritdoc />
        public async UnaryResult<List<Message>>
            GetChatMessagesRecordAsync(ulong sourceId, ulong targetId
                , ulong startTime, ulong endTime)
        {
            return await _noSQLDriven.GetChatMessagesRecordAsync(sourceId
                , targetId, startTime, endTime);
        }

        /// <inheritdoc />
        public async UnaryResult<List<Message>>
            GetTeamMessagesRecordAsync(ulong teamId, ulong startTime
                , ulong endTime)
        {
            return await _noSQLDriven.GetMessagesRecordAsync(teamId, startTime
                , endTime);
        }
    }
}