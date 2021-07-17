﻿using System.Collections.Generic;
using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
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
        private ITalkNoSqlDatabaseDriven _noSQLDriven = new TalkNoSqlDatabaseDriven();

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> SendMessageAsync(Message message)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<bool> SendDataAsync(MessageResponse message, int port, bool respond)
        {
            return await _noSQLDriven.SendDataMessage(message, port);
        }

        /// <inheritdoc />
        public async UnaryResult<MessageResponse> GetMessageAsync(ulong messageId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async UnaryResult<bool> GetDataAsync(MessageResponse message, int port, bool respond)
        {
            return await _noSQLDriven.GetDataMessage(message, port);
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