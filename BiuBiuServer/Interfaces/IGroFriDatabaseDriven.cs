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
        UnaryResult<FriendRequestResponse> WriteFriendRequest(FriendRequest request);

        /// <summary>
        /// 写入群请求
        /// </summary>
        /// <returns>是否成功</returns>
        UnaryResult<GroupRequestResponse> WriteGroupRequest(GroupRequest request);

        /// <summary>
        /// 写入群邀请
        /// </summary>
        /// <returns>是否成功</returns>
        UnaryResult<GroupInvitationResponse> WriteGroupInvitation(GroupInvitation invitation);

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
        /// <param name="request">好友申请</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功</returns>
        UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult);

        /// <summary>
        /// 回复群组邀请
        /// </summary>
        /// <param name="invitation">入群邀请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功/returns>
        UnaryResult<GroupInvitationResponse> ReplyGroupInvitation(GroupInvitation invitation, bool replyResult);

        /// <summary>
        /// 回复入群申请
        /// </summary>
        /// <param name="request">入群申请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功</returns>
        UnaryResult<GroupRequestResponse> ReplyGroupRequest(GroupRequest request, bool replyResult);

        /// <summary>
        /// 退出群聊
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ExitGroup(ulong sponsorId, ulong groupId);

        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DissolveGroup(ulong sponsorId, ulong groupId);

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="targetId">被删除者</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId);

        /// <summary>
        /// 群组踢人
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="targetId">被踢者</param>
        /// <param name="groupId">群组Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong targetId, ulong groupId);
    }
}