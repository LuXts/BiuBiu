using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuServer.TeamHub;
using BiuBiuServer.UserHub;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.Tool;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;
using System.Collections.Generic;

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
                UserHubClient client = new UserHubClient();//定义一个客户端
                await client.ConnectAsync(Initialization.GChannel, request.ReceiverId);//利用信道与目标ID建立联系
                client.ServerSendFriendRequest(request);//操作
                await client.DisposeAsync();
                await client.WaitForDisconnect();//关闭
            }
            return temp;
        }

        public async UnaryResult<TeamRequestResponse> AddGroup(TeamRequest request)
        {
            IdType idType = IdType.TeamRequestId;
            request.RequestId = IdManagement.GenerateId(idType);
            TeamRequestResponse temp = await _igroFriDatabaseDriven.WriteGroupRequest(request);
            if (temp.Success == true)
            {
                UserHubClient client = new UserHubClient();
                TeamInfo teamInfo = await _imInfoDatabaseDriven.GetTeamInfo(request.TeamId);
                await client.ConnectAsync(Initialization.GChannel, teamInfo.OwnerId);
                client.ServerSendGroupRequest(request);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<TeamInvitationResponse> InviteUserToGroup(TeamInvitation invitation)
        {
            IdType idType = IdType.TeamInvitationId;
            invitation.InvitationId = IdManagement.GenerateId(idType);

            var temp = await _igroFriDatabaseDriven.WriteGroupInvitation(invitation);
            if (temp.Success == true)
            {
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(Initialization.GChannel, invitation.ReceiverId);
                client.ServerSendGroupInvitation(invitation);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }

            return temp;
        }

        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetFriendRequest(userId);
        }

        public async UnaryResult<List<TeamInvitation>> GetGroupInvitation(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupInvitation(userId);
        }

        public async UnaryResult<List<TeamRequest>> GetGroupRequest(ulong userId)
        {
            return await _igroFriDatabaseDriven.GetGroupRequest(userId);
        }

        public async UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyFriendRequest(request, replyResult);
            if (temp.Success)
            {
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(Initialization.GChannel, request.SenderId);
                //给申请者发好友申请结果
                request.RequestResult = replyResult.ToString();
                client.ServerSendFriendRequest(request);
                await client.DisposeAsync();
                await client.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<TeamInvitationResponse> ReplyGroupInvitation(TeamInvitation invitation, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyGroupInvitation(invitation, replyResult);
            if (temp.Success)
            {
                var teamInfo = await _imInfoDatabaseDriven.GetTeamInfo(invitation.TeamId);
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(Initialization.GChannel, teamInfo.OwnerId);
                // 给群主发邀请某人入群的结果
                invitation.InvitationResult = replyResult.ToString();
                client.ServerSendGroupInvitation(invitation);
                if (replyResult)
                {
                    // 群发新用户入群消息
                    TeamHubClient client2 = new TeamHubClient();
                    await client2.ConnectAsync(Initialization.GChannel, invitation.TeamId);
                    UserInfoResponse userInfo
                        = await _imInfoDatabaseDriven.GetUserInfo(
                            invitation.ReceiverId);
                    if (userInfo.Success)
                    {
                        client2.ServerAddUser(userInfo);
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

        public async UnaryResult<TeamRequestResponse> ReplyGroupRequest(TeamRequest request, bool replyResult)
        {
            var temp = await _igroFriDatabaseDriven.ReplyGroupRequest(request, replyResult);
            if (temp.Success)
            {
                UserHubClient client = new UserHubClient();
                await client.ConnectAsync(Initialization.GChannel, request.SenderId);
                // 给申请者发入群申请结果
                request.RequestResult = replyResult.ToString();
                client.ServerSendGroupRequest(request);

                if (replyResult)
                {
                    // 群发新用户入群消息
                    TeamHubClient client2 = new TeamHubClient();
                    await client2.ConnectAsync(Initialization.GChannel, request.TeamId);
                    UserInfoResponse userInfo
                        = await _imInfoDatabaseDriven.GetUserInfo(
                            request.SenderId);
                    if (userInfo.Success)
                    {
                        client2.ServerAddUser(userInfo);
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
            var temp = await _igroFriDatabaseDriven.DeleteMemberFromGroup(
                sponsoriInfo.UserId, memberInfo.UserId, teamInfo.TeamId);
            if (temp)
            {
                // 群发消息
                TeamHubClient client2 = new TeamHubClient();
                await client2.ConnectAsync(Initialization.GChannel, teamInfo.TeamId);
                UserInfoResponse userInfo
                    = await _imInfoDatabaseDriven.GetUserInfo(
                        memberInfo.UserId);
                if (userInfo.Success)
                {
                    client2.ServerDelUser(userInfo);
                }
                else
                {
                    Initialization.Logger.Error("查无此人！");
                }
                await client2.DisposeAsync();
                await client2.WaitForDisconnect();
            }
            return temp;
        }

        public async UnaryResult<(bool, ulong)> EstablishTeam(UserInfo builderInfo, TeamInfo teamInfo)
        {
            IdType idType = IdType.TeamId;
            teamInfo.TeamId = IdManagement.GenerateId(idType);//生成群组Id，完善群组信息
            teamInfo.OwnerId = builderInfo.UserId;//根据创建者信息填写群组拥有者Id，完善群组信息
            var re = await _igroFriDatabaseDriven.EstablishTeam(teamInfo);
            return (re, teamInfo.TeamId);
        }
    }
}