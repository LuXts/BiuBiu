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
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.Tool;
using BiuBiuShare.UserHub;
using Grpc.Net.Client;

namespace BiuBiuServer.Services
{
    [Authorize]
    public class GroFriService : ServiceBase<IGroFriService>, IGroFriService
    {
        private readonly IGroFriDatabaseDriven _igroFriDatabaseDriven
            = new GroFriDatabaseDriven();

        private readonly IImInfoDatabaseDriven _imInfoDatabaseDriven = new ImInfoDatabaseDriven();

        public async UnaryResult<bool> AddFriend(ulong senderId, ulong receiverId, string invitationMessage)
        {
            IdType idType = IdType.FriendRequestId;
            ulong friendRequestId = IdManagement.GenerateId(idType);
            bool temp = await _igroFriDatabaseDriven.WriteFriendRequest(friendRequestId, senderId, receiverId,
                invitationMessage);
            if (temp==true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);//利用客户端地址生成一个信道
                UserHubClient client = new UserHubClient();//定义一个客户端
                await client.ConnectAsync(channel, receiverId);//利用信道与目标ID建立联系
                client.SendFriendRequest(new FriendRequest()
                {
                    InvitationMessage = invitationMessage,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    InvitationResult = "",
                    RequestId = friendRequestId
                });//操作
                client.DisposeAsync();
                client.WaitForDisconnect();//关闭
            }

            return temp;
        }

        public async UnaryResult<bool> AddGroup(ulong senderId, ulong groupId, string invitationMessage)
        {
            IdType idType = IdType.TeamRequestId;
            ulong groupRequestId = IdManagement.GenerateId(idType);
            bool temp = await _igroFriDatabaseDriven.WriteGroupRequest(groupRequestId, senderId, groupId,
                invitationMessage);
            if (temp == true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                TeamInfo teamInfo = await _imInfoDatabaseDriven.GetTeamInfo(groupId);
                await client.ConnectAsync(channel, teamInfo.OwnerId);
                client.SendGroupRequest(new GroupRequest()
                {
                    InvitationId = groupRequestId, SenderId = senderId, GroupId = groupId,
                    InvitationMessage = invitationMessage,
                    InvitationResult = ""
                });
                client.DisposeAsync();
                client.WaitForDisconnect();
            }

            return temp;
        }

        public async UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            return await _igroFriDatabaseDriven.DeleteFriend(sponsorId, targetId);
        }

        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong memberId, ulong groupId)
        {
            //TODO:被踢出群聊的人收到消息通知（响应事件）
            bool temp= await _igroFriDatabaseDriven.DeleteMemberFromGroup(sponsorId, memberId, groupId);
            if (temp == true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel,memberId);
            }
            return temp;
        }

        public async UnaryResult<bool> DissolveGroup(ulong userId, ulong groupId)
        {
            return await _igroFriDatabaseDriven.DissolveGroup(userId, groupId);
        }

        public async UnaryResult<bool> ExitGroup(ulong userId, ulong groupId)
        {
            return await _igroFriDatabaseDriven.ExitGroup(userId,groupId);
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

        public async UnaryResult<bool>InviteUserToGroup(ulong senderId, ulong receiverId, ulong groupId,
            string invitationMessage)
        {
            IdType idType = IdType.TeamInvitationId;
            ulong groupInvitationId = IdManagement.GenerateId(idType);

            bool temp= await _igroFriDatabaseDriven.WriteGroupInvitation(groupInvitationId, senderId, receiverId, groupId,
                invitationMessage);
            if (temp==true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel,receiverId);
                client.SendGroupInvitation(new GroupInvitation()
                {
                    InvitationId = groupInvitationId,
                    GroupId = groupId,
                    InvitationResult = "",
                    InvitationMessage = invitationMessage,
                    ReceiverId = receiverId
                });
            }

            return temp;
        }

        public async UnaryResult<bool> ReplyFriendRequest(ulong requestId, bool replyResult)
        {
            //TODO:申请者接收好友申请结果（响应事件）
            bool temp= await _igroFriDatabaseDriven.ReplyFriendRequest(requestId, replyResult);
            return temp;
        }

        public async UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, bool replyResult)
        {
            //TODO:群主收到某人是否同意入群的结果(响应事件)
            return await _igroFriDatabaseDriven.ReplyGroupInvitation(invitationId, replyResult);
        }

        public async UnaryResult<bool> ReplyGroupIRequest(ulong requestId, bool replyResult)
        {
            //TODO：申请者收到入群审核的结果（响应事件）
            return await _igroFriDatabaseDriven.ReplyGroupRequest(requestId, replyResult);
        }
    }
}