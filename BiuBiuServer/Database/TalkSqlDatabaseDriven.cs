using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Database
{
    public class TalkSqlDatabaseDriven : ITalkSqlDatabaseDriven
    {
        // TODO: 记录聊天信息
        // 只需要记录 message.SourceId message.TargetId message.MassageId 三条 Id
        public async UnaryResult<bool> AddMessageAsync(MessageResponse message)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 根据 sourceId targetId 和 startTime endTime 输出 MassageId 的列表
        public async UnaryResult<List<ulong>> GetMessagesRecordAsync(ulong sourceId, ulong targetId
            , ulong startTime, ulong endTime)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 根据 teamId 和 startTime endTime 输出 MassageId 的列表
        public async UnaryResult<List<ulong>> GetTeamMessagesRecordAsync(ulong teamId, ulong startTime
            , ulong endTime)
        {
            throw new System.NotImplementedException();
        }
    }
}