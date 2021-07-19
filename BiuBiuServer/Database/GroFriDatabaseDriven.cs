using BiuBiuServer.Interfaces;
using BiuBiuShare;
using BiuBiuShare.GrouFri;
using MagicOnion;
using System.Collections.Generic;
using NLog.LayoutRenderers;

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

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select UserId from groupconstitute where" +
                                                                   "UserId=?ui,TeamId=?gd",
                new { ui = targetId, gd = groupId });
            if (Target1.Count == 0)
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

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select TeamId from groupconstitute where" +
                                                                   "TeamId=?gd", new { gd = groupId });
            List<ulong> Target2 = await Fsql.Ado.QueryAsync<ulong>("select TeamId from group where" +
                                                                   "TeamId=?gd", new { gd = groupId });
            if (Target2.Count == 0 && Target1.Count == 0)
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
                "TeamId=?gd", new { gd = groupId });
            if (Target[0].Item2 != sponsorId)
            {
                await Fsql.Ado.QueryAsync<object>("delete from groupconstitute where" +
                                                  "UserId=?ui,TeamId=?gd", new { ui = sponsorId, gd = groupId });
            }

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select UserId from groupconstitute where" +
                                                                   "UserId=?ui,TeamId=?gd",
                new { ui = sponsorId, gd = groupId });
            if (Target1.Count == 0)
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
                    "ReceiveId=?rd", new { userId });
            List<FriendRequest> friend = new List<FriendRequest>();
            foreach (var VARIABLE in Target)
            {
                FriendRequest temp = new FriendRequest();
                temp.RequestId = VARIABLE.Item1;
                temp.SenderId = VARIABLE.Item2;
                temp.ReceiverId = VARIABLE.Item3;
                temp.RequestMessage = VARIABLE.Item4;
                temp.RequestResult = VARIABLE.Item5;
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
            foreach (var VARIABLE in Target)
            {
                TeamInvitation temp = new TeamInvitation()
                {
                    InvitationId = VARIABLE.Item1,
                    ReceiverId = VARIABLE.Item3,
                    TeamId = VARIABLE.Item2,
                    InvitationMessage = VARIABLE.Item4,
                    InvitationResult = VARIABLE.Item5
                };

                group.Add(temp);
            }

            return group;
        }

        //TODO:实现请求某用户需要审核的入群申请列表 用户Id 该用户的需要审核的群组申请数组 tip：该用户为群主 即群主获取到加群申请
        public async UnaryResult<List<TeamRequest>> GetGroupRequest(ulong userId)
        {
            List<ulong> GroupOwnerId = await Fsql.Ado.QueryAsync<ulong>("select TeamId from group where" +
                                                                        "OwnerId=?ui", new { ui = userId });
            List<(ulong, ulong, ulong, string, string)> group = new List<(ulong, ulong, ulong, string, string)>();
            foreach (var VARIABLE in GroupOwnerId)
            {
                List<(ulong, ulong, ulong, string, string)> Target =
                    await Fsql.Ado.QueryAsync<(ulong, ulong, ulong, string, string)>(
                        "select ApplyId,TeamId,UserId,Identity,Result from groupapply where" +
                        "TeamId = ?gd", new { gd = VARIABLE });
                foreach (var VARIABLE2 in Target)
                {
                    group.Add(VARIABLE2);
                }
            }

            List<TeamRequest> groupApply = new List<TeamRequest>();
            foreach (var VARIABLE in group)
            {
                TeamRequest temp = new TeamRequest()
                {
                    RequestId = VARIABLE.Item1,
                    SenderId = VARIABLE.Item3,
                    TeamId = VARIABLE.Item2,
                    RequestMessage = VARIABLE.Item4,
                    RequestResult = VARIABLE.Item5
                };

                groupApply.Add(temp);
            }

            return groupApply;
        }

        //TODO:实现回复好友申请 申请ID、回复结果 是否成功修改好友申请信息
        public async UnaryResult<FriendRequestResponse> ReplyFriendRequest(FriendRequest request, bool replyResult)
        {
            throw new System.NotImplementedException();
        }

        //TODO:实现回复群组邀请 邀请ID 回复结果 是否成功修改群组邀请信息
        public async UnaryResult<TeamInvitationResponse> ReplyGroupInvitation(TeamInvitation invitation, bool replyResult)
        {
            throw new System.NotImplementedException();
        }

        //TODO：实现回复入群申请 申请ID 回复结果 是否成功修改入群申请
        public async UnaryResult<TeamRequestResponse> ReplyGroupRequest(TeamRequest request, bool replyResult)
        {
            throw new System.NotImplementedException();
        }

        //TODO:函数功能：输入申请Id，申请者ID，被添加者ID，申请信息，写入数据库中；输出：是否成功 tip:当同一个人邀请信息重复发送时，不重复写入,不可向已是好友关系的人发邀请
        public async UnaryResult<FriendRequestResponse> WriteFriendRequest(FriendRequest request)
        {
            throw new System.NotImplementedException();
        }

        //TODO:实现某人邀请某人加入某群组数据写入，输入邀请Id，邀请者ID，被邀请者ID，群组id,邀请备注信息，写入数据库中；输出：是否成功 tip:不可重复发送、不可邀请在群内的用户
        public async UnaryResult<TeamInvitationResponse> WriteGroupInvitation(TeamInvitation invitation)
        {
            throw new System.NotImplementedException();
        }

        //TODO：实现某人申请加入某群聊数据写入 输入申请Id，申请者ID，群组ID，申请信息 返回是否成功 tip:不可重复写、在群内的人员不可申请
        public async UnaryResult<TeamRequestResponse> WriteGroupRequest(TeamRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}