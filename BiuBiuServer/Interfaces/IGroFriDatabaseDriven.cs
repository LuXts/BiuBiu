using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BiuBiuShare.GrouFri;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    public interface IGroFriDatabaseDriven
    {
        //根据发送者与接收者Id以及备注信息写好友请求
        UnaryResult<int> WriteFriendRequest(ulong friendRequestId,ulong senderId, ulong receiverId, string invitationMessage);
        //根据发送者与群组Id以及备注写加群请求
        UnaryResult<int> WriteGroupRequest(ulong groupRequestId,ulong senderId, ulong groupId, string invitationMessage);
        //根据发送者与接者Id以及备注信息写邀请入群请求
        UnaryResult<int> WriteGroupInvitation(ulong groupInvitationId,ulong senderId, ulong receiver, ulong groupId, string invitationMe);
        //根据用户Id获取用户的好友申请数组
        UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId);
        //根据用户Id获取用户的群组申请数组
        UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId);
        //根据邀请Id和回复结果修改好友申请
        UnaryResult<bool> ReplyFriendRequest(ulong requestId,string replyResult);
        //根据邀请ID和回复结果修改群组邀请
        UnaryResult<bool> ReplyGroupInvitation(ulong invitationId,string replyResult);
        //根据邀请ID和审核结果修改入群申请
        UnaryResult<bool> ReplyGroupRequest(ulong requestId,string replyResult);
        //根据发起人Id和群组Id进行退群
        UnaryResult<bool> ExitGroup(ulong sponsorId,ulong groupId);
        //根据发起人Id和群组Id进行解散群聊
        UnaryResult<bool> DissolveGroup(ulong sponsorId,ulong groupId);
        //根据发起人Id和目标Id进行删除好友
        UnaryResult<bool> DeleteFriend(ulong sponsorId,ulong targetId);
        //根据发起人Id和群组id以及目标Id进行踢人操作
        UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId,ulong targetId,ulong groupId);
    }
}