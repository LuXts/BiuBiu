using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Tool;
using MagicOnion;
using System.Collections.Generic;

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
                        " UserId = ?ui ", new { ui = userId.ToString() });

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
                "select ChangeId from userchange where" + " UserId=?ui"
                , new { ui = userId.ToString() });
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
                            " UserId=?ui", new { ui = userId.ToString() });
            }
            else
            {
                Target
                    = await Fsql.Ado
                        .QueryAsync<(ulong, string, string, string, string,
                            string, ulong, string, string)>(
                            "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin,Password from userchange where" +
                            " UserId=?ui", new { ui = userId.ToString() });
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

        //修改用户信息
        // userInfo.UserId 就是用户 ID
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> SetUserInfo(
            UserInfo userInfo)
        {
            List<(ulong, string)> user = await Fsql.Ado.QueryAsync<(ulong, string)>("select UserId,Password from user where" +
                                                                " UserId=?ui", new { ui = userInfo.UserId.ToString() });
            List<ulong> change = await Fsql.Ado.QueryAsync<ulong>("select UserId from userchange where" +
                                                                  " UserId=?ui", new { ui = userInfo.UserId.ToString() });

            if (user.Count != 0 && change.Count == 0)
            {
                var temp = user[0];
                IdType type = IdType.ModifyId;
                ulong changeId = IdManagement.GenerateId(type);
                await Fsql.Ado.QueryAsync<object>(
                    "insert into userchange values(?ui,?dn,?jn,?dp,?pn,?em,?ic,false,?pd,?cd)",
                    new
                    {
                        ui = userInfo.UserId.ToString(),
                        dn = userInfo.DisplayName,
                        jn = userInfo.JobNumber,
                        dp = userInfo.Description,
                        pn = userInfo.PhoneNumber,
                        em = userInfo.Email,
                        ic = userInfo.IconId.ToString(),
                        pd = temp.Item2,
                        cd = changeId.ToString()
                    });
            }
            else if (user.Count != 0 && change.Count != 0)
            {
                await Fsql.Ado.QueryAsync<object>(
                    "update userchange set DisplayName=?dn,JobNumber=?jn,Description=?dp," +
                    "PhoneNumber=?pn,Email=?em,Icon=?ic where" +
                    " UserId = ?ui",
                    new
                    {
                        dn = userInfo.DisplayName,
                        jn = userInfo.JobNumber,
                        dp = userInfo.Description,
                        pn = userInfo.PhoneNumber,
                        em = userInfo.Email,
                        ic = userInfo.IconId.ToString(),
                        ui = userInfo.UserId.ToString()
                    });
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select ChangeId from userchange where" +
                                                                  " UserId=?ui", new { ui = userInfo.UserId.ToString() });

            if (Target.Count == 0)
            {
                return UserInfoResponse.Failed;
            }
            else
            {
                return new UserInfoResponse() { Success = true };
            }
        }

        //修改用户密码
        // 这个输入参数简单
        /// <inheritdoc />
        public async UnaryResult<UserInfoResponse> SetUserPassword(ulong userId
            , string oldPassword, string newPassword)
        {
            List<string> Target = await Fsql.Ado.QueryAsync<string>(
                "select Password from user where UserId = ?ui"
                , new { ui = userId.ToString() });
            if (Target.Count != 0)
            {
                if (Target[0] == oldPassword)
                {
                    await Fsql.Ado.QueryAsync<object>(
                        "update user set Password = ?pd where UserId=?ui"
                        , new { pd = newPassword, ui = userId.ToString() });
                }
            }

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>(
                "select UserId from user where Password = ?pd and Userid = ?ui"
                , new { pd = newPassword, ui = userId.ToString() });

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
                        "select GroupId,GroupName,Description,Icon,OwnerId from team where" +
                        " GroupId=?gd", new { gd = teamId.ToString() });
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

        // 设置群组信息
        /// <inheritdoc />
        public async UnaryResult<TeamInfoResponse> SetTeamInfo(TeamInfo teamInfo)
        {
            // 同上
            // 如果没成功就返回 return TeamInfoResponse.Failed;
            // 如果成功就返回 return new TeamInfoResponse(){ Success = true };
            List<ulong> group = await Fsql.Ado.QueryAsync<ulong>("select GroupId from team where GroupId=?gd",
                new { gd = teamInfo.TeamId.ToString() });

            if (group.Count != 0)
            {
                await Fsql.Ado.QueryAsync<object>("update team set GroupName=?gn,Description=?dp,Icon=?ic,OwnerId=?od where" +
                                                  " GroupId=?gd",
                                                  new
                                                  {
                                                      gn = teamInfo.TeamName,
                                                      dp = teamInfo.Description,
                                                      ic = teamInfo.IconId.ToString(),
                                                      od = teamInfo.OwnerId.ToString(),
                                                      gd = teamInfo.TeamId.ToString()
                                                  });
            }

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select GroupId from team where" +
                                                                  " GroupId=?gd and GroupName=?gn and Description=?dp and Icon=?ic and OwnerId=?od",
                new
                {
                    gd = teamInfo.TeamId.ToString(),
                    gn = teamInfo.TeamName,
                    dp = teamInfo.Description,
                    ic = teamInfo.IconId.ToString(),
                    od = teamInfo.OwnerId.ToString()
                });

            if (Target.Count == 0)
            {
                return TeamInfoResponse.Failed;
            }
            else
            {
                return new TeamInfoResponse() { Success = true };
            }
        }

        //获取群员信息（连表查询）
        /// <inheritdoc />
        public async UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            List<(ulong, string, string, string, string, string, ulong, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select u.UserId,u.DisplayName,u.JobNumber,u.Description,u.PhoneNumber,u.Email,u.Icon,u.IsAdmin from user u,groupconstitute g where" +
                    " g.GroupId=?gd and u.UserId = g.UserId", new { gd = teamId.ToString() });

            List<UserInfo> user = new List<UserInfo>();

            foreach (var VARIABLE in Target)
            {
                UserInfo temp = new UserInfo()
                {
                    UserId = VARIABLE.Item1,
                    DisplayName = VARIABLE.Item2,
                    JobNumber = VARIABLE.Item3,
                    Description = VARIABLE.Item4,
                    PhoneNumber = VARIABLE.Item5,
                    Email = VARIABLE.Item6,
                    IconId = VARIABLE.Item7
                };

                if (VARIABLE.Item8 == "false")
                {
                    temp.Permissions = false;
                }
                else
                {
                    temp.Permissions = true;
                }

                user.Add(temp);
            }

            return user;
        }

        //获取好友信息
        public async UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId)
        {
            List<(ulong, string, string, string, string, string, ulong, string)> Target1 =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select u.UserId,u.DisplayName,u.JobNumber,u.Description,u.PhoneNumber,u.Email,u.Icon,u.IsAdmin from user u,friendrelation f where" +
                    " f.SendId = ?ui and f.ReceiveId = u.UserId", new { ui = userId.ToString() });
            List<(ulong, string, string, string, string, string, ulong, string)> Target2 =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select u.UserId,u.DisplayName,u.JobNumber,u.Description,u.PhoneNumber,u.Email,u.Icon,u.IsAdmin from user u,friendrelation f where" +
                    " f.ReceiveId = ?ui and f.SendId = u.UserId", new { ui = userId.ToString() });
            List<UserInfo> user = new List<UserInfo>();

            foreach (var VARIABLE in Target1)
            {
                UserInfo temp = new UserInfo()
                {
                    UserId = VARIABLE.Item1,
                    DisplayName = VARIABLE.Item2,
                    JobNumber = VARIABLE.Item3,
                    Description = VARIABLE.Item4,
                    PhoneNumber = VARIABLE.Item5,
                    Email = VARIABLE.Item6,
                    IconId = VARIABLE.Item7
                };

                if (VARIABLE.Item8 == "false")
                {
                    temp.Permissions = false;
                }
                else
                {
                    temp.Permissions = true;
                }

                user.Add(temp);
            }

            foreach (var VARIABLE in Target2)
            {
                UserInfo temp = new UserInfo()
                {
                    UserId = VARIABLE.Item1,
                    DisplayName = VARIABLE.Item2,
                    JobNumber = VARIABLE.Item3,
                    Description = VARIABLE.Item4,
                    PhoneNumber = VARIABLE.Item5,
                    Email = VARIABLE.Item6,
                    IconId = VARIABLE.Item7
                };

                if (VARIABLE.Item8 == "false")
                {
                    temp.Permissions = false;
                }
                else
                {
                    temp.Permissions = true;
                }

                user.Add(temp);
            }

            return user;
        }

        //获取某人全部群聊信息
        public async UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong userId)
        {
            List<(ulong, string, string, ulong, ulong)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, ulong, ulong)>(
                    "select g.GroupId,g.GroupName,g.Description,g.Icon,g.OwnerId from team g,groupconstitute c where" +
                    " g.GroupId=c.GroupId and c.UserId=?ui", new { ui = userId.ToString() });
            List<TeamInfo> group = new List<TeamInfo>();
            foreach (var VARIABLE in Target)
            {
                TeamInfo temp = new TeamInfo()
                {
                    TeamId = VARIABLE.Item1,
                    TeamName = VARIABLE.Item2,
                    Description = VARIABLE.Item3,
                    IconId = VARIABLE.Item4,
                    OwnerId = VARIABLE.Item5
                };
                group.Add(temp);
            }

            return group;
        }

        //获取某用户的最后登录时间 （那个一直没用上的字段）
        // 查不到用户就返回 0
        public async UnaryResult<ulong> GetUserLastLoginTime(ulong userId)
        {
            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select LastExitTime from user where UserId=?ui",
                new { ui = userId.ToString() });

            if (Target.Count == 0)
            {
                return 0;
            }
            else
            {
                return Target[0];
            }
        }

        //设置某用户的最后登录时间 （那个一直没用上的字段）
        // 通过 bool 返回是否成功
        public async UnaryResult<bool> SetUserLastLoginTime(ulong userId, ulong lastLoginTime)
        {
            await Fsql.Ado.QueryAsync<object>("update user set LastExitTime = ?lt where UserId = ?ui",
                new { lt = lastLoginTime.ToString(), ui = userId.ToString() });

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>(
                "select UserId from user where UserId=?ui,LastExitTime=?lt",
                new { ui = userId.ToString(), lt = lastLoginTime.ToString() });

            if (Target.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}