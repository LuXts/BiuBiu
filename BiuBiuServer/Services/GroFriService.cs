using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
using BiuBiuShare;
using BiuBiuShare.UserManagement;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using System;
using System.Threading;
using BiuBiuServer.Database;
using MagicOnion.Server.Authentication;
using System.Collections.Generic;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ServiceInterfaces;

namespace BiuBiuServer.Services
{
    [Authorize]
    public class GroFriService : ServiceBase<IGroFriService>, IGroFriService
    {
        private readonly IGroFriDatabaseDriven _igroFriDatabaseDriven
            = new GroFriDatabaseDriven();

        public async UnaryResult<int> AddFriend(ulong senderId, ulong receiverId, string invitationMessage)
        {
            //TODO:接受者接收消息消息、生成唯一Id
            ulong friendRequestId = 0;
            return await _igroFriDatabaseDriven.WriteFriendRequest(friendRequestId, senderId, receiverId, invitationMessage);
        }

        public async UnaryResult<int> AddGroup(ulong senderId, ulong groupId, string invitationMessage)
        {
            //TODO:群主接收申请消息、生成唯一ID
            ulong groupRequestId = 0;
            return await _igroFriDatabaseDriven.WriteGroupRequest(groupRequestId, senderId, groupId, invitationMessage);
        }

        public async UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            return await _igroFriDatabaseDriven.DeleteFriend(sponsorId, targetId);
        }

        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong memberId, ulong groupId)
        {
            return await _igroFriDatabaseDriven.DeleteMemberFromGroup(sponsorId, memberId, groupId);
        }

        public async UnaryResult<bool> DissolveGroup(ulong userId, ulong groupId)
        {
            return await _igroFriDatabaseDriven.DissolveGroup(userId, groupId);
        }

        public async UnaryResult<bool> ExitGroup(ulong userId, ulong groupId)
        {
            return await _igroFriDatabaseDriven.DeleteFriend(userId, groupId);
        }

        public async UnaryResult<List<GroupInvitation>> GetFriendInvitation(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupInvitation(userId);
        }

        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetFriendRequest(userId);
        }

        public async UnaryResult<int> InviteUserToGroup(ulong senderId, ulong receiverId, ulong groupId, string invitationMessage)
        {
            //TODO：被邀请者接收申请消息、生成唯一ID
            ulong groupInvitationId = 0;
            return await _igroFriDatabaseDriven.WriteGroupInvitation(groupInvitationId, senderId, receiverId, groupId, invitationMessage);
        }

        public async UnaryResult<bool> ReplyFriendRequest(ulong requestId, string replyResult)
        {
            return await _igroFriDatabaseDriven.ReplyFriendRequest(requestId, replyResult);
        }

        public async UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, string replyResult)
        {
            return await _igroFriDatabaseDriven.ReplyGroupInvitation(invitationId, replyResult);
        }

        public async UnaryResult<bool> ReplyGroupIRequest(ulong requestId, string replyResult)
        {
            return await _igroFriDatabaseDriven.ReplyGroupRequest(requestId, replyResult);
        }
    }
}