using System.Collections.Generic;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 聊天服务与 SQL 数据库之间的驱动
    /// </summary>
    public interface ITalkSqlDatabaseDriven
    {
        public UnaryResult<bool> AddMessageAsync(
            MessageResponse messageResponse);

        public UnaryResult<List<ulong>> GetMessagesRecordAsync(ulong sourceId
            , ulong targetId, ulong startTime, ulong endTime);

        public UnaryResult<List<ulong>> GetTeamMessagesRecordAsync(ulong teamId
            , ulong startTime, ulong endTime);
    }
}