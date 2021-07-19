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
using BiuBiuShare.SignIn;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.TeamHub;
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

        public async UnaryResult<FriendRequestResponse> AddFriend(FriendRequest request)
        {
            IdType idType = IdType.FriendRequestId;
            request.RequestId = IdManagement.GenerateId(idType);
            FriendRequestResponse temp = await _igroFriDatabaseDriven.WriteFriendRequest(request);
            if (temp.Success == true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);//生成一个信道
                UserHubClient client = new UserHubClient();//定义一个客户端
                await client.ConnectAsync(channel, request.ReceiverId);//利用信道与目标ID建立联系
                client.SendFriendRequest(request);//操作
                await client.DisposeAsync();
                await client.WaitForDisconnect();//关闭
            }
            return temp;
        }

        public async UnaryResult<GroupRequestResponse> AddGroup(GroupRequest request)
        {
            IdType idType = IdType.TeamRequestId;
            request.RequestId = IdManagement.GenerateId(idType);
            GroupRequestResponse temp = await _igroFriDatabaseDriven.WriteGroupRequest(request);
            if (temp.Success == true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                TeamInfo teamInfo = await _imInfoDatabaseDriven.GetTeamInfo(request.GroupId);
                await client.ConnectAsync(channel, teamInfo.OwnerId);
                client.SendGroupRequest(request);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<GroupInvitationResponse> InviteUserToGroup(GroupInvitation invitation)
        {
            IdType idType = IdType.TeamInvitationId;
            invitation.InvitationId = IdManagement.GenerateId(idType);

            var temp = await _igroFriDatabaseDriven.WriteGroupInvitation(invitation);
            if (temp.Success == true)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel, invitation.ReceiverId);
                client.SendGroupInvitation(invitation);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }

            return temp;
        }

        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetFriendRequest(userId);
        }

        public async UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupInvitation(userId);
        }

        public async UnaryResult<List<GroupRequest>> GetGroupRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupRequest(userId);
        }

        public async UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyFriendRequest(request, replyResult);
            if (temp.Success)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel, request.SenderId);
                //给申请者发好友申请结果
                client.SendFriendRequest(request);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<GroupInvitationResponse> ReplyGroupInvitation(GroupInvitation invitation, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyGroupInvitation(invitation, replyResult);
            if (temp.Success)
            {
                var teamInfo = await _imInfoDatabaseDriven.GetTeamInfo(invitation.GroupId);
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel, teamInfo.OwnerId);
                // 给群主发邀请某人入群的结果
                client.SendGroupInvitation(invitation);
                if (replyResult)
                {
                    // 群发新用户入群消息
                    TeamHubClient client2 = new TeamHubClient();
                    await client2.ConnectAsync(channel, invitation.GroupId);
                    UserInfoResponse userInfo
                        = await _imInfoDatabaseDriven.GetUserInfo(
                            invitation.ReceiverId);
                    if (userInfo.Success)
                    {
                        client2.AddUser(userInfo);
                    }
                    else
                    {
                        Initialization.Logger.Error("查无此人！");
                    }
                    await client2.DisposeAsync();
                    await client2.WaitForDisconnect();
                }
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<GroupRequestResponse> ReplyGroupRequest(GroupRequest request, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyGroupRequest(request, replyResult);
            if (temp.Success)
            {
                var channel = GrpcChannel.ForAddress(Initialization.GrpcAddress);

                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(channel, request.SenderId);
                // 给申请者发入群申请结果
                client.SendGroupRequest(request);

                if (replyResult)
                {
                    // 群发新用户入群消息
                    TeamHubClient client2 = new TeamHubClient();
                    await client2.ConnectAsync(channel, request.GroupId);
                    UserInfoResponse userInfo
                        = await _imInfoDatabaseDriven.GetUserInfo(
                            request.SenderId);
                    if (userInfo.Success)
                    {
                        client2.AddUser(userInfo);
                    }
                    else
                    {
                        Initialization.Logger.Error("查无此人！");
                    }
                    await client2.DisposeAsync();
                    await client2.WaitForDisconnect();
                }
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<bool> ExitGroup(UserInfo userInfo, TeamInfo teamInfo)
        {
            return await _igroFriDatabaseDriven.ExitGroup(userInfo.UserId, teamInfo.TeamId);
        }

        public async UnaryResult<bool> DissolveGroup(UserInfo ownerInfo, TeamInfo teamInfo)
        {
            return await _igroFriDatabaseDriven.DissolveGroup(ownerInfo.UserId, teamInfo.TeamId);
        }

        public async UnaryResult<bool> DeleteFriend(UserInfo sponsorInfo, UserInfo targetInfo)
        {
            return await _igroFriDatabaseDriven.DeleteFriend(sponsorInfo.UserId
                , targetInfo.UserId);
        }

        public async UnaryResult<bool> DeleteMemberFromGroup(UserInfo sponsoriInfo, UserInfo memberInfo
            , TeamInfo teamInfo)
        {
            return await _igroFriDatabaseDriven.DeleteMemberFromGroup(
                sponsoriInfo.UserId, memberInfo.UserId, teamInfo.TeamId);
        }
    }
}