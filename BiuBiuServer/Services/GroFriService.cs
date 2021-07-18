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
using BiuBiuShare.Tool;
using Grpc.Net.Client;

namespace BiuBiuServer.Services
{
    [Authorize]
    public class GroFriService : ServiceBase<IGroFriService>, IGroFriService
    {
        private readonly IGroFriDatabaseDriven _igroFriDatabaseDriven
            = new GroFriDatabaseDriven();

        public async UnaryResult<int> AddFriend(ulong senderId, ulong receiverId, string invitationMessage)
        {
            //TODO:接受者接收消息消息(响应事件)
            IdType idType = IdType.FriendRequestId;
            ulong friendRequestId = IdManagement.GenerateId(idType);
            return await _igroFriDatabaseDriven.WriteFriendRequest(friendRequestId, senderId, receiverId,
                invitationMessage);
        }

        public async UnaryResult<int> AddGroup(ulong senderId, ulong groupId, string invitationMessage)
        {
            //TODO:群主接收申请消息(响应事件)
            IdType idType = IdType.GroupRequestId;
            ulong groupRequestId = IdManagement.GenerateId(idType);
            int re = await _igroFriDatabaseDriven.WriteGroupRequest(groupRequestId, senderId, groupId,
                invitationMessage);
            if (re == 1)
            {
                // TODO:
            }
            else
            {
            }
            return re;
        }

        public async UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            return await _igroFriDatabaseDriven.DeleteFriend(sponsorId, targetId);
        }

        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong memberId, ulong groupId)
        {
            //TODO:被踢出群聊的人收到消息通知（响应事件）
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

        public async UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupInvitation(userId);
        }

        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetFriendRequest(userId);
        }

        public async UnaryResult<List<GroupRequest>> GetGroupRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupRequest(userId);
        }

        public async UnaryResult<int> InviteUserToGroup(ulong senderId, ulong receiverId, ulong groupId,
            string invitationMessage)
        {
            //TODO：被邀请者接收申请信息(响应事件)
            IdType idType = IdType.GroupInvitationId;
            ulong groupInvitationId = IdManagement.GenerateId(idType);
            ;
            return await _igroFriDatabaseDriven.WriteGroupInvitation(groupInvitationId, senderId, receiverId, groupId,
                invitationMessage);
        }

        public async UnaryResult<bool> ReplyFriendRequest(ulong requestId, string replyResult)
        {
            //TODO:申请者接收好友申请结果（响应事件）
            return await _igroFriDatabaseDriven.ReplyFriendRequest(requestId, replyResult);
        }

        public async UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, string replyResult)
        {
            //TODO:群主收到某人是否同意入群的结果(响应事件)
            return await _igroFriDatabaseDriven.ReplyGroupInvitation(invitationId, replyResult);
        }

        public async UnaryResult<bool> ReplyGroupIRequest(ulong requestId, string replyResult)
        {
            //TODO：申请者收到入群审核的结果（响应事件）
            return await _igroFriDatabaseDriven.ReplyGroupRequest(requestId, replyResult);
        }
    }
}