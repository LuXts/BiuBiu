using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using BiuBiuShare.UserManagement;
using MagicOnion;
using Microsoft.AspNetCore.Mvc;

namespace BiuBiuServer.Database
{
    public class AdminDatabaseDriven : IAdminDatabaseDriven
    {
        private readonly IFreeSql Fsql = MySqlDriven.GetFreeSql();

        //函数功能：管理员修改用户密码 输入：用户Id、新密码 输出：修改密码是否成功
        public async UnaryResult<bool> ChangePassword(ulong userId, string newPassword)
        {
            await Fsql.Ado.QueryAsync<object>("update user set Password = ?np where UserId = ?ui",
                new { np = newPassword, ui = userId.ToString() });

            List<string> Target =
                await Fsql.Ado.QueryAsync<string>("select Password from user where UserId = ?ui", new { ui = userId.ToString() });

            if (Target[0] == newPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //函数功能：管理员修改用户信息 输入：用户ID、新的用户信息 输出：是否修改成功
        public async UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo)
        {
            string permission;
            if (newUserInfo.Permissions)
            {
                permission = "true";
            }
            else
            {
                permission = "false";
            }
            await Fsql.Ado.QueryAsync<object>(
                "update user set DisplayName = ?un,JobNumber = ?jn,Description = ?up,PhoneNumber = ?pn,Email = ?em,Icon = ?ic,IsAdmin = ?isa where UserId = ?ui",
                new
                {
                    un = newUserInfo.DisplayName,
                    jn = newUserInfo.JobNumber,
                    up = newUserInfo.Description,
                    pn = newUserInfo.PhoneNumber,
                    em = newUserInfo.Email,
                    ic = newUserInfo.IconId.ToString(),
                    isa = permission,
                    ui = newUserInfo.UserId.ToString()
                });
            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                  " where DisplayName = ?un and JobNumber = ?jn and Description = ?up and PhoneNumber = ?pn and Email = ?em and Icon = ?ic and IsAdmin = ?isa and UserId = ?ui",
                new
                {
                    un = newUserInfo.DisplayName,
                    jn = newUserInfo.JobNumber,
                    up = newUserInfo.Description,
                    pn = newUserInfo.PhoneNumber,
                    em = newUserInfo.Email,
                    ic = newUserInfo.IconId.ToString(),
                    isa = newUserInfo.Permissions.ToString(),
                    ui = newUserInfo.UserId.ToString()
                });

            if (Target.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //函数功能：删除用户 输入：用户Id 输出：是否成功
        public async UnaryResult<bool> DeleteUser(ulong userId)
        {
            await Fsql.Ado.QueryAsync<object>("delete from user where UserId = ?ui", new { ui = userId.ToString() });

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                  " where UserId = ?ui", new { ui = userId.ToString() });
            if (Target.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取待审核列表
        /// </summary>
        /// <returns></returns>
        public async UnaryResult<List<UserInfo>> GetModifyInfo()
        {
            List<(ulong, string, string, string, string, string, ulong, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin from userchange");

            List<UserInfo> user = new List<UserInfo>();

            foreach (var VARIABLE in Target)
            {
                bool IsAdmin;
                if (VARIABLE.Item8 == "false")
                {
                    IsAdmin = false;
                }
                else
                {
                    IsAdmin = true;
                }
                UserInfo temp = new UserInfo()
                {
                    UserId = VARIABLE.Item1,
                    DisplayName = VARIABLE.Item2,
                    JobNumber = VARIABLE.Item3,
                    Description = VARIABLE.Item4,
                    PhoneNumber = VARIABLE.Item5,
                    Email = VARIABLE.Item6,
                    IconId = VARIABLE.Item7,
                    Permissions = IsAdmin
                };

                user.Add(temp);
            }

            return user;
        }

        //函数功能：根据ID和注册信息注册新用户 输入：注册用户信息 输出：注册信息提示信息(-1表示该工号被注册，-2表示该手机号码被注册，0表示数据库因故障未插入成功,1表示成功)
        public async UnaryResult<int> RegisteredUsers(ulong userId, RegisterInfo registerInfos)
        {
            int mark;

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   " where JobNumber = ?jn",
                new { jn = registerInfos.JobNumber });

            List<ulong> Target2 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   " where PhoneNumber = ?pn",
                new { pn = registerInfos.PhoneNumber });
            if (Target1.Count != 0)
            {
                mark = -1;
            }
            else if (Target2.Count != 0)
            {
                mark = -2;
            }
            else
            {
                mark = 1;
            }

            if (mark == 1)
            {
                string isAdmin = "";
                if (registerInfos.Permissions)
                {
                    isAdmin = "true";
                }
                else
                {
                    isAdmin = "false";
                }

                await Fsql.Ado.QueryAsync<object>("insert into user values (?ui,?dn,?jn,null,?pn,null,'1705905438572183552',?isa,?pd,'0')",
                    new
                    {
                        ui = userId.ToString(),
                        dn = registerInfos.UserName,
                        jn = registerInfos.JobNumber,
                        pn = registerInfos.PhoneNumber,
                        isa = isAdmin,
                        pd = "123456"
                    });
            }

            List<ulong> Target3 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   " where UserId = ?ui", new { ui = userId.ToString() });
            if (Target3.Count == 0)
            {
                mark = 0;
            }

            return mark;
        }

        //函数功能：审核消息 输入：用户Id与审核结果（0表示不通过，1表示通过）,根据审核结果修改用户数据 输出：是否成功
        public async UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            if (reviewResults)
            {
                List<(string, string, string, string, string, ulong)> Target =
                    await Fsql.Ado.QueryAsync<(string, string, string, string, string, ulong)>(
                        "Select DisplayName,JobNumber,Description,PhoneNumber,Email,Icon from userchange where UserId=?ui",
                        new { ui = userId.ToString() });
                await Fsql.Ado.QueryAsync<object>(
                    "update user set DisplayName = ?un,JobNumber = ?jn,Description = ?up,PhoneNumber = ?pn,Email = ?em,Icon = ?ic where UserId = ?ui ",
                    new
                    {
                        un = Target[0].Item1,
                        jn = Target[0].Item2,
                        up = Target[0].Item3,
                        pn = Target[0].Item4,
                        em = Target[0].Item5,
                        ic = Target[0].Item6.ToString(),
                        ui = userId.ToString()
                    });
                List<ulong> Target2 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                       " where DisplayName = ?un and JobNumber = ?jn and Description = ?up and PhoneNumber = ?pn and Email = ?em and Icon = ?ic",
                    new
                    {
                        un = Target[0].Item1,
                        jn = Target[0].Item2,
                        up = Target[0].Item3,
                        pn = Target[0].Item4,
                        em = Target[0].Item5,
                        ic = Target[0].Item6.ToString()
                    });
                if (Target2.Count == 0)
                {
                    return false;
                }
                else
                {
                    await Fsql.Ado.QueryAsync<object>("delete from userchange where UserId = ?ui", new { ui = userId.ToString() });
                    return true;
                }
            }
            else
            {
                await Fsql.Ado.QueryAsync<object>("delete from userchange where UserId = ?ui", new { ui = userId.ToString() });
                List<ulong> Target =
                    await Fsql.Ado.QueryAsync<ulong>("select UserId from userchange where UserId = ?ui",
                        new { ui = userId.ToString() });
                if (Target.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //函数功能：按照工号查找用户 输入：用户工号 输出：用户信息
        public async UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            List<(ulong, string, string, string, string, string, ulong, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin from user" +
                    " where JobNumber=?jn", new { jn = jobNumber });

            if (Target.Count != 0)
            {
                bool IsAdmin;
                if (Target[0].Item8 == "true")
                {
                    IsAdmin = true;
                }
                else
                {
                    IsAdmin = false;
                }
                var user = new UserInfo();
                var temp = Target[0];

                user.UserId = temp.Item1;
                user.DisplayName = temp.Item2;
                user.JobNumber = temp.Item3;
                user.Description = temp.Item4;
                user.PhoneNumber = temp.Item5;
                user.Email = temp.Item6;
                user.IconId = temp.Item7;
                user.Permissions = IsAdmin;

                return user;
            }
            else
            {
                return null;
            }
        }

        //函数功能：按照用户Id查找用户 输入：用户Id 输出：用户信息
        public async UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            List<(ulong, string, string, string, string, string, ulong, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string, ulong, string)>(
                    "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email,Icon,IsAdmin from user" +
                    " where UserId = ?ui", new { ui = userId.ToString() });

            bool IsAdmin;

            if (Target.Count != 0)
            {
                if (Target[0].Item8 == "true")
                {
                    IsAdmin = true;
                }
                else
                {
                    IsAdmin = false;
                }
                var user = new UserInfo();
                var temp = Target[0];

                user.UserId = temp.Item1;
                user.DisplayName = temp.Item2;
                user.JobNumber = temp.Item3;
                user.Description = temp.Item4;
                user.PhoneNumber = temp.Item5;
                user.Email = temp.Item6;
                user.IconId = temp.Item7;
                user.Permissions = IsAdmin;

                return user;
            }
            else
            {
                return null;
            }
        }
    }
}