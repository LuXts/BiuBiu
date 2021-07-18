using BiuBiuServer.Interfaces;
using BiuBiuShare;
using BiuBiuShare.GrouFri;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuServer.Database
{
    public class GroFriDatabaseDriven:IGroFriDatabaseDriven
    {
        //TODO:实现删除好友操作 发起者Id、目标Id 是否成功
        public async UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现踢人操作 发起者Id，目标Id，群组Id 是否成功 tip：必须是该群的群主才能踢人
        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong targetId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现解散群聊 发起者Id，群组Id 是否成功 tip：必须是该群的群主才能解散群聊
        public async UnaryResult<bool> DissolveGroup(ulong sponsorId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现退出群聊 发起者Id、群组Id 是否成功 tip：群主不能退出群聊
        public async UnaryResult<bool> ExitGroup(ulong sponsorId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }

        //TODO:实现请求某用户的好友申请列表 用户ID 该用户的全部好友申请数组
        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            throw new System.NotImplementedException();
        }
        //TODO:实现请求某用户的群组邀请列表 用户ID 该用户的全部群组邀请数组
        public async UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId)
        {
            throw new System.NotImplementedException();
        }
        //TODO:实现回复好友申请 申请ID、回复结果（“接受”“拒绝””忽略“） 是否成功修改好友申请信息
        public async UnaryResult<bool> ReplyFriendRequest(ulong requestId, string replyResult)
        {
            throw new System.NotImplementedException();
        }
        //TODO:实现回复群组邀请 邀请ID 回复结果（接受、拒绝、忽略） 是否成功修改群组邀请信息
        public async UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, string replyResult)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现回复入群申请 申请ID 回复结果同上 是否成功修改入群申请
        public async UnaryResult<bool> ReplyGroupRequest(ulong requestId, string replyResult)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：输入申请Id，申请者ID，被添加者ID，申请信息，写入数据库中；输出：返回int标识是否成功与失败原因tip:当同一个人邀请信息重复发送时，不重复写入,返回失败
        public async UnaryResult<int> WriteFriendRequest(ulong friendRequestId, ulong senderId, ulong receiverId, string invitationMessage)
        {
            throw new System.NotImplementedException();
        }
        //TODO:实现某人邀请某人加入某群组数据写入，输入邀请Id，邀请者ID，被邀请者ID，群组id,邀请备注信息，写入数据库中；输出：返回int标识是否成功与失败原因 tip:邀请人必须为群主，不可重复发送
        public async UnaryResult<int> WriteGroupInvitation(ulong groupInvitationId, ulong senderId, ulong receiver, ulong groupId, string invitationMe)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现某人申请加入某群聊数据写入 输入申请Id，申请者ID，群组ID，申请信息 返回int标识是否成功与失败原因 tip:不可重复写
        public async UnaryResult<int> WriteGroupRequest(ulong groupRequestId, ulong senderId, ulong groupId, string invitationMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}