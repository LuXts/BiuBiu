using BiuBiuShare.TalkInfo;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuShare.ServiceInterfaces
{
    /// <summary>
    /// 聊天相关服务的接口
    /// </summary>
    public interface ITalkService : IService<ITalkService>
    {
        /// <summary>
        /// 发送信息的函数
        /// </summary>
        /// <param name="message">消息类的实例</param>
        /// <returns>
        /// 返回生成的消息Id，消息内容等。
        /// 请根据返回的 <c>messageResponse.Success</c> 判断是否失败。
        /// </returns>
        public UnaryResult<(MessageResponse, uint)> SendMessageAsync(Message message);

        /// <summary>
        /// 发送非文本类消息后根据 Response 继续发送数据
        /// </summary>
        /// <param name="message">上一步获得的消息</param>
        /// <param name="port">上传端口</param>
        /// <param name="respond">是否继续上传</param>
        /// <returns>上传是否成功</returns>
        public UnaryResult<MessageResponse> SendDataAsync(Message message, uint port, bool respond);

        /// <summary>
        /// 查询某条消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>
        /// 返回查到的消息内容等。
        /// 请根据返回的 <c>messageResponse.Success</c> 判断是否失败。
        /// </returns>
        public UnaryResult<(MessageResponse, uint)> GetMessageAsync(Message message);

        /// <summary>
        /// 发送 GetMessageAsync 消息后根据 Response 的结果接收数据
        /// </summary>
        /// <param name="message">上一步获得的消息</param>
        /// <param name="port">下载端口</param>
        /// <param name="respond">是否继续下载</param>
        /// <returns>返回是否成功</returns>
        public UnaryResult<MessageResponse> GetDataAsync(Message message, uint port, bool respond);

        /// <summary>
        /// 获取某Id的未读消息（其实就是某时间点之后的所有消息）
        /// </summary>
        /// <param name="targetId">目标Id</param>
        /// <param name="startTime">开始时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <param name="endTime">结束时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <returns>
        /// 返回消息列表，消息列表为空时代表没查到
        /// </returns>
        public UnaryResult<List<Message>> GetNoReadMessageRecordAsync(
            ulong targetId, ulong startTime, ulong endTime);

        /// <summary>
        /// 获取一段时间内的私聊聊天记录
        /// </summary>
        /// <param name="sourceId">私聊中的一个人Id</param>
        /// <param name="targetId">私聊中的另一个人Id</param>
        /// <param name="startTime">开始时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <param name="endTime">结束时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <returns>
        /// 返回消息列表，消息列表为空时代表没查到
        /// </returns>
        public UnaryResult<List<Message>> GetChatMessagesRecordAsync(
            ulong sourceId, ulong targetId, ulong startTime, ulong endTime);

        // 获取一段时间内 TeamId 里面的聊天记录
        /// <summary>
        /// 获取一段时间内的群聊聊天记录
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <param name="startTime">开始时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <param name="endTime">结束时间（通过 <c>BiuBiuShare.Tool.IdManagement</c> 转换）</param>
        /// <returns>
        /// 返回消息列表，消息列表为空时代表没查到
        /// </returns>
        public UnaryResult<List<Message>> GetTeamMessagesRecordAsync(
            ulong teamId, ulong startTime, ulong endTime);
    }
}