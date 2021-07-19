using BiuBiuServer.Interfaces;
using BiuBiuShare;
using BiuBiuShare.GrouFri;
using MagicOnion;
using System.Collections.Generic;
using BiuBiuShare.Tool;
using NLog.LayoutRenderers;
using BiuBiuShare.ImInfos;

namespace BiuBiuServer.Database
{
    public class GroFriDatabaseDriven : IGroFriDatabaseDriven
    {
        private readonly IFreeSql Fsql = MySqlDriven.GetFreeSql();

        //实现删除好友操作 发起者Id、目标Id 是否成功
        public async UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                                   "SendId = ?sd,ReceiveId = ?rd",
                new { sd = sponsorId, rd = targetId });
            if (Target1.Count != 0)
            {
                await Fsql.Ado.QueryAsync<object>("delete from friendrelation where SendId = ?sd,ReceiveId = ?rd",
                    new { sd = sponsorId, rd = targetId });
            }

            List<ulong> Target2 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                                   "SendId = ?sd,ReceiveId = ?rd",
                new { sd = targetId, rd = sponsorId });
            if (Target2.Count != 0)
            {
                await Fsql.Ado.QueryAsync<object>("delete from friendrelation where SendId = ?sd,ReceiveId = ?rd",
                    new { sd = targetId, rd = sponsorId });
            }
            Target1.Clear();
            Target1 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                       "SendId = ?sd,ReceiveId = ?rd",
                new { sd = sponsorId, rd = targetId });
            Target2.Clear();
            Target2 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                       "SendId = ?sd,ReceiveId = ?rd",
                new { sd = targetId, rd = sponsorId });
            if (Target1.Count == 0 && Target2.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //实现踢人操作 发起者Id，目标Id，群组Id 是否成功 tip：必须是该群的群主才能踢人
        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong targetId, ulong groupId)
        {
            List<(ulong, ulong)> Target = await Fsql.Ado.QueryAsync<(ulong, ulong)>(
                "select TeamId,OwnerId from group where" +
                "TeamId=?gd", new { gd = groupId });
            if (Target[0].Item2 == sponsorId)
            {
                await Fsql.Ado.QueryAsync<object>("delete from groupconstitute where UserId=?ui,TeamId=?gd",
                    new { ui = targetId, gd = groupId });
            }

            List<ulong> list = await Fsql.Ado.QueryAsync<ulong>("select UserId from groupconstitute where" +
                                                                   "UserId=?ui,TeamId=?gd",
                new { ui = targetId, gd = groupId });
            if (list.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //实现解散群聊 发起者Id，群组Id 是否成功 tip：必须是该群的群主才能解散群聊
        public async UnaryResult<bool> DissolveGroup(ulong sponsorId, ulong groupId)
        {
            List<(ulong, ulong)> Target = await Fsql.Ado.QueryAsync<(ulong, ulong)>(
                "select TeamId,OwnerId from group where" +
                "TeamId=?gd", new { gd = groupId });

            if (Target[0].Item2 == sponsorId)
            {
                await Fsql.Ado.QueryAsync<object>("delete from groupconstitute where" +
                                                  "TeamId=?gd", new { gd = groupId });
                await Fsql.Ado.QueryAsync<object>("delete from group where" +
                                                  "TeamId=?gd", new { gd = groupId });
            }

            List<ulong> list1 = await Fsql.Ado.QueryAsync<ulong>("select TeamId from groupconstitute where" +
                                                                   "TeamId=?gd", new { gd = groupId });
            List<ulong> list2 = await Fsql.Ado.QueryAsync<ulong>("select TeamId from group where" +
                                                                   "TeamId=?gd", new { gd = groupId });
            if (list2.Count == 0 && list1.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //实现退出群聊 发起者Id、群组Id 是否成功 tip：群主不能退出群聊
        public async UnaryResult<bool> ExitGroup(ulong sponsorId, ulong groupId)
        {
            List<(ulong, ulong)> Target = await Fsql.Ado.QueryAsync<(ulong, ulong)>(
                "select TeamId,OwnerId from group where" +
                "TeamId=?gd", new {gd = groupId});
            if (Target[0].Item2 != sponsorId)
            {
                await Fsql.Ado.QueryAsync<object>("delete from groupconstitute where" +
                                                  "UserId=?ui,TeamId=?gd", new { ui = sponsorId, gd = groupId });
            }

            List<ulong> list = await Fsql.Ado.QueryAsync<ulong>("select UserId from groupconstitute where" +
                                                                   "UserId=?ui,TeamId=?gd",
                new { ui = sponsorId, gd = groupId });
            if (list.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //实现请求某用户的好友申请列表 用户ID 该用户的全部好友申请数组
        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            List<(ulong, ulong, ulong, string, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, ulong, ulong, string, string)>(
                    "Select AddId,SendId,ReceiveId,Identity,Result from friendadd where" +
                    "ReceiveId=?rd", new {rd = userId});
            List<FriendRequest> friend = new List<FriendRequest>();
            foreach (var tuple in Target)
            {
                FriendRequest temp = new FriendRequest();
                temp.RequestId = tuple.Item1;
                temp.SenderId = tuple.Item2;
                temp.ReceiverId = tuple.Item3;
                temp.RequestMessage = tuple.Item4;
                temp.RequestResult = tuple.Item5;
                friend.Add(temp);
            }

            return friend;
        }

        //实现请求某用户的群组邀请列表 用户ID 该用户的全部群组邀请数组
        public async UnaryResult<List<TeamInvitation>> GetGroupInvitation(ulong userId)
        {
            List<(ulong, ulong, ulong, string, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, ulong, ulong, string, string)>(
                    "select InviteId,TeamId,UserId,Identity,Result from groupinvite where" +
                    "Userid=?ui", new { ui = userId });

            List<TeamInvitation> group = new List<TeamInvitation>();
            foreach (var tuple in Target)
            {
                TeamInvitation temp = new TeamInvitation()
                {
                    InvitationId = tuple.Item1,
                    ReceiverId = tuple.Item3,
                    TeamId = tuple.Item2,
                    InvitationMessage = tuple.Item4,
                    InvitationResult = tuple.Item5
                };

                group.Add(temp);
            }

            return group;
        }

        //实现请求某用户需要审核的入群申请列表 用户Id 该用户的需要审核的群组申请数组 tip：该用户为群主 即群主获取到加群申请
        public async UnaryResult<List<TeamRequest>> GetGroupRequest(ulong userId)
        {
            List<ulong> groupOwnerId = await Fsql.Ado.QueryAsync<ulong>("select TeamId from group where" +
                                                                        "OwnerId=?ui", new { ui = userId });
            List<(ulong, ulong, ulong, string, string)> group = new List<(ulong, ulong, ulong, string, string)>();
            foreach (var ownerId in groupOwnerId)
            {
                List<(ulong, ulong, ulong, string, string)> Target =
                    await Fsql.Ado.QueryAsync<(ulong, ulong, ulong, string, string)>(
                        "select ApplyId,TeamId,UserId,Identity,Result from groupapply where" +
                        "TeamId = ?gd", new { gd = ownerId });
                foreach (var item in Target)
                {
                    group.Add(item);
                }
            }

            List<TeamRequest> groupApply = new List<TeamRequest>();
            foreach (var tuple in group)
            {
                TeamRequest temp = new TeamRequest()
                {
                    RequestId = tuple.Item1,
                    SenderId = tuple.Item3,
                    TeamId = tuple.Item2,
                    RequestMessage = tuple.Item4,
                    RequestResult = tuple.Item5
                };

                groupApply.Add(temp);
            }

            return groupApply;
        }

        //实现回复好友申请 申请ID、回复结果 是否成功修改好友申请信息
        public async UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult)
        {
            List<(ulong, ulong)> friendApply = await Fsql.Ado.QueryAsync<(ulong, ulong)>(
                "select SendId,ReceiveId from friendadd where" +
                "AddId=?ad", new {ad = request.RequestId});
            if (friendApply.Count != 0)
            {
                if (replyResult)
                {
                    IdType type = IdType.FriendRelationId;
                    ulong relationId = IdManagement.GenerateId(type);
                    await Fsql.Ado.QueryAsync<object>("insert into friendrelation values(?sd,?rd,?red)",
                        new {sd = request.SenderId, rd = request.ReceiverId, red = relationId});
                }
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                                  "SendId=?sd,ReceiveId=?rd",
                new {sd = request.SenderId, rd = request.ReceiverId});

            await Fsql.Ado.QueryAsync<object>("delete from friendadd where" +
                                              "AddId=?ad", new {ad = request.RequestId});

            if (Target.Count == 0)
            {
                return FriendRequestResponse.Failed;
            }
            else
            {
                return new FriendRequestResponse() {Success = true};
            }

        }

        //实现回复群组邀请 邀请ID 回复结果 是否成功修改群组邀请信息
        public async UnaryResult<TeamInvitationResponse> ReplyGroupInvitation(TeamInvitation invitation, bool replyResult)
        {
            List<ulong> groupRelation = await Fsql.Ado.QueryAsync<ulong>("select InviteId from groupinvite where" +
                                                                         "InviteId = ?iid",
                new {iid = invitation.InvitationId});
            if (groupRelation.Count != 0)
            {
                if (replyResult)
                {
                    IdType type = IdType.TeamRelationId;
                    ulong relationId = IdManagement.GenerateId(type);
                    await Fsql.Ado.QueryAsync<object>("Insert into groupconstitute values(?ui,?gd,?it,?red)",
                        new
                        {
                            ui = invitation.ReceiverId, 
                            gd = invitation.TeamId, 
                            it = invitation.InvitationMessage,
                            red = relationId
                        });
                }
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select RelationId from groupconstitute where" +
                                                                  "UserId=?ui,GroupId=?gd",
                new {ui = invitation.ReceiverId, gd = invitation.TeamId});

            await Fsql.Ado.QueryAsync<object>("delete from groupinvite where" +
                                              "InviteId=?iid", new {iid = invitation.InvitationId});

            if (Target.Count == 0)
            {
                return TeamInvitationResponse.Failed;;
            }
            else
            {
                return new TeamInvitationResponse() {Success = true};
            }
        }

        //实现回复入群申请 申请ID 回复结果 是否成功修改入群申请
        public async UnaryResult<TeamRequestResponse> ReplyGroupRequest(TeamRequest request, bool replyResult)
        {
            List<ulong> groupRelation = await Fsql.Ado.QueryAsync<ulong>("select ApplyId from groupapply where" +
                                                                         "ApplyId=?ad", new {ad = request.RequestId});
            if (groupRelation.Count != 0)
            {
                if (replyResult)
                {
                    IdType type = IdType.TeamRelationId;
                    ulong relationId = IdManagement.GenerateId(type);
                    await Fsql.Ado.QueryAsync<object>("Insert into groupconstitute values(?ui,?gd,?it,?red)",
                        new
                        {
                            ui = request.SenderId, 
                            gd = request.TeamId,
                            it = request.RequestMessage, 
                            red = relationId
                        });
                }
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select RelationId from groupconstitute where" +
                                                                  "UserId=?ui,GroupId=?gd",
                new {ui = request.SenderId, gd = request.TeamId});

            await Fsql.Ado.QueryAsync<object>("delete from groupapply where" +
                                              "ApplyId=?ad", new {ad = request.RequestId});

            if (Target.Count == 0)
            {
                return TeamRequestResponse.Failed;
            }
            else
            {
                return new TeamRequestResponse() {Success = true};
            }
        }

        //函数功能：输入申请Id，申请者ID，被添加者ID，申请信息，写入数据库中；输出：是否成功 tip:当同一个人邀请信息重复发送时，不重复写入,不可向已是好友关系的人发邀请
        public async UnaryResult<FriendRequestResponse> WriteFriendRequest(FriendRequest request)
        {
            List<ulong> IsFriend1 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                                     "SendId=?sd,ReceiveId=?rd",
                new {sd = request.SenderId, rd = request.ReceiverId});
            List<ulong> IsFriend2 = await Fsql.Ado.QueryAsync<ulong>("select RelationId from friendrelation where" +
                                                                     "SendId=?sd,ReceiveId=?rd",
                new { sd = request.ReceiverId, rd = request.SenderId });
            List<ulong> hasAdd1 = await Fsql.Ado.QueryAsync<ulong>("select AddId from friendadd where" +
                                                                   "SendId=?sd,ReceiveId=?rd",
                new {sd = request.SenderId, rd = request.ReceiverId});
            List<ulong> hasAdd2 = await Fsql.Ado.QueryAsync<ulong>("select AddId from friendadd where" +
                                                                   "SendId=?sd,ReceiveId=?rd",
                new { sd = request.ReceiverId, rd = request.SenderId });

            if (IsFriend1.Count == 0 && IsFriend2.Count == 0 && hasAdd1.Count == 0 && hasAdd2.Count == 0)
            {
                await Fsql.Ado.QueryAsync<object>("insert into friendadd values (?ad,?sd,?rd,?rt,?it)",
                    new
                    {
                        ad = request.RequestId, sd = request.SenderId, rd = request.ReceiverId,
                        rt = request.RequestResult, it = request.RequestMessage
                    });
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select AddId from friendadd where" +
                                                                  "SendId=?sd,ReceiveId=?rd",
                new { sd = request.SenderId, rd = request.ReceiverId });

            if (Target.Count == 0)
            {
                return FriendRequestResponse.Failed;
            }
            else
            {
                return new FriendRequestResponse() {Success = true};
            }
        }

        //实现某人邀请某人加入某群组数据写入，输入邀请Id，邀请者ID，被邀请者ID，群组id,邀请备注信息，写入数据库中；输出：是否成功 tip:不可重复发送、不可邀请在群内的用户
        public async UnaryResult<TeamInvitationResponse> WriteGroupInvitation(TeamInvitation invitation)
        {
            List<ulong> InGroup = await Fsql.Ado.QueryAsync<ulong>("select RelationId from groupconstitute where" +
                                                                   "UserId=?ui,GroupId=?gd",
                new {ui = invitation.ReceiverId, gd = invitation.TeamId});

            List<ulong>hasInvite = await Fsql.Ado.QueryAsync<ulong>("select InviteId from groupinvite where" +
                                                                    "UserId=?ui,GroupId=?gd",
                new { ui = invitation.ReceiverId, gd = invitation.TeamId });
            List<ulong>hasApply = await Fsql.Ado.QueryAsync<ulong>("select ApplyId from groupapply where" +
                                                                   "UserId=?ui,GroupId=?gd",
                new { ui = invitation.ReceiverId, gd = invitation.TeamId });

            if (InGroup.Count == 0 && hasInvite.Count == 0 && hasApply.Count == 0)
            {
                await Fsql.Ado.QueryAsync<object>("insert into groupinvite values (?iid,?gd,?ui,?it,?rt)",
                    new
                    {
                        iid = invitation.InvitationId, gd = invitation.TeamId, ui = invitation.ReceiverId,
                        it = invitation.InvitationMessage, rt = invitation.InvitationResult
                    });
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select InviteId from groupinvite where" +
                                                                  "UserId=?ui,GroupId=?gd",
                new { ui = invitation.ReceiverId, gd = invitation.TeamId });

            if (Target.Count == 0)
            {
                return TeamInvitationResponse.Failed;
            }
            else
            {
                return new TeamInvitationResponse() {Success = true};
            }
        }

        //实现某人申请加入某群聊数据写入 输入申请Id，申请者ID，群组ID，申请信息 返回是否成功 tip:不可重复写、在群内的人员不可申请
        public async UnaryResult<TeamRequestResponse> WriteGroupRequest(TeamRequest request)
        {
            List<ulong> InGroup = await Fsql.Ado.QueryAsync<ulong>("select RelationId from groupconstitute where" +
                                                                   "UserId=?ui,GroupId=?gd",
                new {ui = request.SenderId, gd = request.TeamId});

            List<ulong> hasInvite = await Fsql.Ado.QueryAsync<ulong>("select InviteId from groupinvite where" +
                                                                     "UserId=?ui,GroupId=?gd",
                new { ui = request.SenderId, gd = request.TeamId });
            List<ulong> hasApply = await Fsql.Ado.QueryAsync<ulong>("select ApplyId from groupapply where" +
                                                                    "UserId=?ui,GroupId=?gd",
                new { ui = request.SenderId, gd = request.TeamId });

            if (InGroup.Count == 0 && hasInvite.Count == 0 && hasApply.Count == 0)
            {
                await Fsql.Ado.QueryAsync<object>("insert into groupapply values (?ad,?gd,?ui,?it,?rt)",
                    new
                    {
                        ad = request.RequestId, gd = request.TeamId, ui = request.SenderId, it = request.RequestMessage,
                        rt = request.RequestResult
                    });
            }

            List<ulong>Target = await Fsql.Ado.QueryAsync<ulong>("select ApplyId from groupapply where" +
                                                                 "UserId=?ui,GroupId=?gd",
                new { ui = request.SenderId, gd = request.TeamId });

            if (Target.Count == 0)
            {
                return TeamRequestResponse.Failed;
            }
            else
            {
                return new TeamRequestResponse() {Success = true};
            }
        }

        //TODO ：实现建立群聊 群组信息 返回是否成功
        public async UnaryResult<bool> EstablishTeam(TeamInfo teamInfo)
        {
            throw new System.NotImplementedException();
        }

    }
}