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

namespace BiuBiuServer.Services
{
    public class GroFriService : ServiceBase<IGroFriService>, IGroFriService
    {
        private readonly IGroFriDatabaseDriven _igroFriDatabaseDriven
            = new GroFriDatabaseDriven();

        public async  UnaryResult<int> AddFriend(ulong senderId, ulong receiverId, string invitationMessage)
        {
            //TODO:接受者接收消息消息
            return await _igroFriDatabaseDriven.WriteFriendRequest(senderId,receiverId,invitationMessage);
        }

        public async UnaryResult<int> AddGroup(ulong senderId, ulong groupId, string invitationMessage)
        {
            //TODO:群主接收申请消息
            return await _igroFriDatabaseDriven.WriteGroupRequest(senderId, groupId, invitationMessage);
        }

        public async UnaryResult<int> InviteUserToGroup(ulong senderId, ulong receiverId, ulong groupId, string invitationMessage)
        {
            //TODO：被邀请者接收申请消息
            return await _igroFriDatabaseDriven.WriteGroupInvitation(senderId,receiverId,groupId,invitationMessage);
        }
    }
}