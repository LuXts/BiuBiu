using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BiuBiuShare.GrouFri;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 好友以及群组相关操作数据库驱动接口
    /// </summary>
    public interface IGroFriDatabaseDriven
    {
        /// <summary>
        ///写好友申请 
        /// </summary>
        /// <param name="friendRequestId">好友申请Id</param>
        /// <param name="senderId">发送者</param>
        /// <param name="receiverId">接收者</param>
        /// <param name="invitationMessage">申请备注信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> WriteFriendRequest(ulong friendRequestId,ulong senderId, ulong receiverId, string invitationMessage);

        /// <summary>
        /// 写入群请求
        /// </summary>
        /// <param name="groupRequestId">入群请求Id</param>
        /// <param name="senderId">发送者Id</param>
        /// <param name="groupId">群Id</param>
        /// <param name="invitationMessage">备注信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> WriteGroupRequest(ulong groupRequestId,ulong senderId, ulong groupId, string invitationMessage);
        
        /// <summary>
        /// 写入群邀请
        /// </summary>
        /// <param name="groupInvitationId">入群邀请Id</param>
        /// <param name="senderId">发送者Id</param>
        /// <param name="receiver">接受者Id</param>
        /// <param name="groupId">群Id</param>
        /// <param name="invitationMe">备注</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> WriteGroupInvitation(ulong groupInvitationId,ulong senderId, ulong receiver, ulong groupId, string invitationMe);
        
        /// <summary>
        /// 获取用户收到的好友申请
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>好友申请列表</returns>
        UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId);
        
        /// <summary>
        /// 获取用户收到的群组邀请
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>群组邀请列表</returns>
        UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId);
        
        /// <summary>
        /// 获取用户收到的入群申请
        /// </summary>
        /// <param name="userId">用户</param>
        /// <returns>入群申请列表</returns>
        UnaryResult<List<GroupRequest>> GetGroupRequest(ulong userId);
        
        /// <summary>
        /// 回复好友申请
        /// </summary>
        /// <param name="requestId">好友申请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ReplyFriendRequest(ulong requestId,bool replyResult);
        
        /// <summary>
        /// 回复群组邀请
        /// </summary>
        /// <param name="invitationId">入群邀请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功/returns>
        UnaryResult<bool> ReplyGroupInvitation(ulong invitationId,bool replyResult);
        
        /// <summary>
        /// 回复入群申请
        /// </summary>
        /// <param name="requestId">入群申请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ReplyGroupRequest(ulong requestId,bool replyResult);
        
        /// <summary>
        /// 退出群聊
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ExitGroup(ulong sponsorId,ulong groupId);
        
        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DissolveGroup(ulong sponsorId,ulong groupId);
        
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="targetId">被删除者</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteFriend(ulong sponsorId,ulong targetId);
        
        /// <summary>
        /// 群组踢人
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="targetId">被踢者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId,ulong targetId,ulong groupId);
    }
}