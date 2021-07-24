using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using MagicOnion;
using System.Collections.Generic;

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
        UnaryResult<TeamRequestResponse> WriteGroupRequest(TeamRequest request);

        /// <summary>
        /// 写入群邀请
        /// </summary>
        /// <returns>是否成功</returns>
        UnaryResult<TeamInvitationResponse> WriteGroupInvitation(TeamInvitation invitation);

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
        UnaryResult<List<TeamInvitation>> GetGroupInvitation(ulong userId);

        /// <summary>
        /// 获取用户收到的入群申请
        /// </summary>
        /// <param name="userId">用户</param>
        /// <returns>入群申请列表</returns>
        UnaryResult<List<TeamRequest>> GetGroupRequest(ulong userId);

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
        UnaryResult<TeamInvitationResponse> ReplyGroupInvitation(TeamInvitation invitation, bool replyResult);

        /// <summary>
        /// 回复入群申请
        /// </summary>
        /// <param name="request">入群申请Id</param>
        /// <param name="replyResult">是否同意</param>
        /// <returns>是否成功</returns>
        UnaryResult<TeamRequestResponse> ReplyGroupRequest(TeamRequest request, bool replyResult);

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

        /// <summary>
        /// 建立群聊
        /// </summary>
        /// <param name="teamInfo">群信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> EstablishTeam(TeamInfo teamInfo);
    }
}