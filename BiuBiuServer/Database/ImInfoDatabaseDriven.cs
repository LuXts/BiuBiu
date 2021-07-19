using System;
using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuServer.Database
{
    /// <summary>
    /// 数据库驱动实现
    /// </summary>
    public class ImInfoDatabaseDriven : IImInfoDatabaseDriven
    {
        private readonly IFreeSql Fsql = MySqlDriven.GetFreeSql();

        //输入用户ID，获取用户信息
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> GetUserInfo(ulong userId)
        {
            List<(ulong, string, string, string, string, string, ulong, string,
                string)> Target
                = await Fsql.Ado
                    .QueryAsync<(ulong, string, string, string, string, string,
                        ulong, string, string)>(
                        "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin,Password from user where" +
                        "UserId=?ui", new { ui = userId });

            if (Target.Count == 0)
            {
                return UserInfoResponse.Failed;
            }

            var temp = Target[0];
            UserInfoResponse user = new UserInfoResponse()
            {
                UserId = temp.Item1
                ,
                DisplayName = temp.Item2
                ,
                JobNumber = temp.Item3
                ,
                Description = temp.Item4
                ,
                PhoneNumber = temp.Item5
                ,
                Email = temp.Item6
                ,
                IconId = temp.Item7
                ,
                Success = true
            };
            if (temp.Item8 == "true")
            {
                user.Permissions = true;
            }
            else
            {
                user.Permissions = false;
            }

            return user;
        }

        // 获取修改队列里面的用户信息
        // 就是如果有之前未审核的修改申请，就拿那个修改申请的值回来，
        // 否则和上面那个函数返回值一样
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> GetModifyQueueInfo(
            ulong userId)
        {
            List<ulong> change = await Fsql.Ado.QueryAsync<ulong>(
                "select ChangeId from userchange where" + "UserId=?ui"
                , new { ui = userId });
            List<(ulong, string, string, string, string, string, ulong, string,
                string)> Target
                = new List<(ulong, string, string, string, string, string, ulong
                    , string, string)>();
            if (change.Count == 0)
            {
                Target
                    = await Fsql.Ado
                        .QueryAsync<(ulong, string, string, string, string,
                            string, ulong, string, string)>(
                            "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin,Password from user where" +
                            "UserId=?ui", new { ui = userId });
            }
            else
            {
                Target
                    = await Fsql.Ado
                        .QueryAsync<(ulong, string, string, string, string,
                            string, ulong, string, string)>(
                            "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin,Password from userchange where" +
                            "UserId=?ui", new { ui = userId });
            }

            if (Target.Count == 0)
            {
                return UserInfoResponse.Failed;
            }

            var temp = Target[0];
            UserInfoResponse user = new UserInfoResponse()
            {
                UserId = temp.Item1
                ,
                DisplayName = temp.Item2
                ,
                JobNumber = temp.Item3
                ,
                Description = temp.Item4
                ,
                PhoneNumber = temp.Item5
                ,
                Email = temp.Item6
                ,
                IconId = temp.Item7
                ,
                Success = true
            };
            if (temp.Item8 == "true")
            {
                user.Permissions = true;
            }
            else
            {
                user.Permissions = false;
            }

            return user;
        }

        // TODO: 修改用户信息
        // userInfo.UserId 就是用户 ID
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> SetUserInfo(
            UserInfo userInfo)
        {
            // 如果没成功就返回 return UserInfoResponse.Failed;
            // 如果成功就返回 return new UserInfoResponse(userInfo){ Success = true };
            // 类名里面带个 Response 的都会有 Success 字段
            throw new System.NotImplementedException();
        }

        //修改用户密码
        // 这个输入参数简单
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> SetUserPassword(ulong userId
            , string oldPassword, string newPassword)
        {
            List<string> Target = await Fsql.Ado.QueryAsync<string>(
                "select Password from user where UserId = ?ui"
                , new { ui = userId });
            if (Target.Count != 0)
            {
                if (Target[0] == oldPassword)
                {
                    await Fsql.Ado.QueryAsync<object>(
                        "update user set Password = ?pd where UserId=?ui"
                        , new { pd = newPassword, ui = userId });
                }
            }

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>(
                "select UserId from user where Password = ?pd and Userid = ?ui"
                , new { pd = newPassword, ui = userId });

            if (Target1.Count == 0)
            {
                return UserInfoResponse.Failed;
            }
            else
            {
                return new UserInfoResponse() { Success = true };
            }
        }

        //获取群组信息
        /// <inheritdoc />
        public async UnaryResult<TeamInfoResponse> GetTeamInfo(ulong teamId)
        {
            List<(ulong, string, string, ulong, ulong)> Target
                = await Fsql.Ado
                    .QueryAsync<(ulong, string, string, ulong, ulong)>(
                        "select TeamId,GroupName,Description,Icon,OwnerId from Group where" +
                        "TeamId=?gd", new { gd = teamId });
            if (Target.Count == 0)
            {
                return TeamInfoResponse.Failed;
            }
            var temp = Target[0];
            TeamInfoResponse team = new TeamInfoResponse()
            {
                TeamId = temp.Item1
                ,
                TeamName = temp.Item2
                ,
                Description = temp.Item3
                ,
                OwnerId = temp.Item5
                ,
                IconId = temp.Item4
                ,
                Success = true
            };

            return team;
        }

        // TODO: 设置群组信息
        /// <inheritdoc />
        public async UnaryResult<TeamInfoResponse> SetTeamInfo(TeamInfo teamInfo)
        {
            // 同上
            // 如果没成功就返回 return TeamInfoResponse.Failed;
            // 如果成功就返回 return new TeamInfoResponse(){ Success = true };
            throw new System.NotImplementedException();
        }

        // TODO: 获取群员信息（连表查询）
        /// <inheritdoc />
        public async UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 获取好友信息
        public async UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId)
        {
            throw new NotImplementedException();
        }

        // TODO: 获取某人全部群聊信息
        public async UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong userId)
        {
            throw new NotImplementedException();
        }
    }
}