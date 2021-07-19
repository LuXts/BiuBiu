using System.Collections.Generic;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 聊天服务与 NoSQL 数据库之间的驱动
    /// </summary>
    public interface ITalkNoSqlDatabaseDriven
    {
        // 把聊天记录加入数据库
        public UnaryResult<MessageResponse> AddMessageAsync(Message message);

        // 获取聊天记录
        public UnaryResult<MessageResponse> GetMessagesAsync(Message message);

        // 发送文件
        public UnaryResult<MessageResponse> SendDataMessage(Message message
            , uint port);

        // 接收文件
        public UnaryResult<MessageResponse> GetDataMessage(Message message
            , uint port);

        // 获取一段时间内未接收的聊天记录
        // targetId 可以为 UserId 也可以为 TeamId
        public UnaryResult<List<Message>> GetMessagesRecordAsync(
            ulong targetId, ulong startTime, ulong endTime);

        // 获取一段时间内的私聊聊天记录
        public UnaryResult<List<Message>> GetChatMessagesRecordAsync(ulong sourceId
            , ulong targetId, ulong startTime, ulong endTime);
    }
}