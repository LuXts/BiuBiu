using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 聊天服务与 NoSQL 数据库之间的驱动
    /// </summary>
    public interface ITalkNoSqlDatabaseDriven
    {
        public UnaryResult<bool> AddMessageAsync(MessageResponse message);

        public UnaryResult<MessageResponse> GetMessagesAsync(ulong messageId);

        public UnaryResult<bool> SendDataMessage(MessageResponse message
            , int port);

        public UnaryResult<bool> GetDataMessage(ulong messageId, int port);
    }
}