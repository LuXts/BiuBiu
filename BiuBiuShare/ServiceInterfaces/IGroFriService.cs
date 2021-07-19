using System.Collections.Generic;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using MagicOnion;
using Org.BouncyCastle.Bcpg;

namespace BiuBiuShare.ServiceInterfaces
{
    /// <summary>
    /// 用户群组相关操作接口
    /// </summary>
    public interface IGroFriService : IService<IGroFriService>
    {
        /// <summary>
        /// 申请添加好友
        /// </summary>
        /// <param name="senderId">申请者Id</param>
        /// <param name="receiverId">被申请者Id</param>
        /// <param name="invitationMessage">申请好友时的备注信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<FriendRequestResponse> AddFriend(FriendRequest request);

        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="senderId">申请者Id</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="invitationMessage">申请加入群组时的备注信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<TeamRequestResponse> AddGroup(TeamRequest request);

        /// <summary>
        /// 邀请某人加入某群组
        /// </summary>
        /// <param name="senderId">发起邀请者Id</param>
        /// <param name="receiverId">被邀请者Id</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="invitationMessage">邀请加入群聊的备注信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<TeamInvitationResponse> InviteUserToGroup(TeamInvitation invitation);

        /// <summary>
        /// 获取某个用户所收到的好友申请
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>好友申请数组</returns>
        UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId);

        /// <summary>
        /// 获取某个用户收到的群组邀请
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>群组邀请数组</returns>
        UnaryResult<List<TeamInvitation>> GetGroupInvitation(ulong userId);

        /// <summary>
        /// 获取用户需要审核的加群申请
        /// </summary>
        /// <param name="userId">发起邀请者Id</param>
        /// <returns>加群申请数组</returns>
        UnaryResult<List<TeamRequest>> GetGroupRequest(ulong userId);

        /// <summary>
        /// 回复添加好友请求
        /// </summary>
        /// <param name="userId">申请者Id </param>
        /// <param name="requestId">好友请求Id</param>
        /// <param name="replyResult">是否同意该用户的好友申请</param>
        /// <returns>是否成功</returns>
        UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult);//此Id表示申请者的Id信息，用于给申请者发好友申请结果

        /// <summary>
        /// 回复邀请加群请求
        /// </summary>
        /// <param name="userId">群主Id</param>
        /// <param name="invitationId">加群邀请Id</param>
        /// <param name="replyResult">是否同意该用户的入群邀请</param>
        /// <returns>是否成功</returns>
        UnaryResult<TeamInvitationResponse> ReplyGroupInvitation(TeamInvitation invitation, bool replyResult);//此Id表示群主的Id信息，用于给群主发邀请入群结果

        /// <summary>
        /// 回复加群请求
        /// </summary>
        /// <param name="userId">申请者Id</param>
        /// <param name="requestId">加群请求Id</param>
        /// <param name="replyResult">是否同意该用户加群</param>
        /// <returns>是否成功</returns>
        UnaryResult<TeamRequestResponse> ReplyGroupRequest(TeamRequest request, bool replyResult);//此Id表示申请者Id，用于给申请者发入群申请结果

        /// <summary>
        /// 退出群聊
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">群Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ExitGroup(UserInfo userInfo, TeamInfo teamInfo);

        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">群Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DissolveGroup(UserInfo ownerInfo, TeamInfo teamInfo);

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="sponsorId">发起者删除请求者</param>
        /// <param name="targetId">被删除者</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteFriend(UserInfo sponsorInfo, UserInfo targetInfo);

        /// <summary>
        /// 群组踢人
        /// </summary>
        /// <param name="sponsorId">发起者</param>
        /// <param name="memberId">被踢者</param>
        /// <param name="groupId">群Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteMemberFromGroup(UserInfo sponsoriInfo, UserInfo memberInfo, TeamInfo teamInfo);
    }
}