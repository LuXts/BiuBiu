using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare;
using BiuBiuShare.Response;
using BiuBiuShare.TalkInfo;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    // TODO: 数据库驱动逻辑未完成
    /// <summary>
    /// 聊天相关服务实现
    /// </summary>
    [Authorize]
    public class TalkService : ServiceBase<ITalkService>, ITalkService
    {
        private ITalkSqlDatabaseDriven _sqlDriven;
        private ITalkNoSqlDatabaseDriven _noSQLDriven;

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> SendMessageAsync(Message message)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<bool> SendDataAsync(MessageResponse message, bool respond)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> GetMessageAsync(ulong messageId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<bool> GetDataAsync(MessageResponse message, bool respond)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<List<MessageResponse>> GetMessagesRecordAsync(ulong SourceId, ulong TargetId
            , ulong startTime, ulong endTime)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<List<MessageResponse>> GetTeamMessagesRecordAsync(ulong TeamId, ulong startTime
            , ulong endTime)
        {
            throw new System.NotImplementedException();
        }
    }
}