using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.UserManagement;
using MagicOnion;
using Microsoft.AspNetCore.Mvc;

namespace BiuBiuServer.Database
{
    public class AdminDatabaseDriven : IAdminDatabaseDriven
    {
        private IFreeSql Fsql = MySqlDriven.GetFreeSql();
        //TODO:函数功能：管理员修改用户密码 输入：用户Id、新密码 输出：修改密码是否成功
        public async UnaryResult<bool> ChangePassword(ulong userId, string newPassword)
        {
            await Fsql.Ado.QueryAsync<object>("update user set Password = ?np where UserId = ?ui",
                new {nm = newPassword, ui = userId});

            List<string> Target =
                await Fsql.Ado.QueryAsync<string>("select Password from user where UserId = ?ui", new {ui = userId});

            if (Target[0] == newPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //TODO:函数功能：管理员修改用户信息 输入：用户ID、新的用户信息 输出：是否修改成功
        public async UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo)
        {
            
            await Fsql.Ado.QueryAsync<object>(
                "update user set DisplayName = ?un,JobNumber = ?jn,Description = ?up,PhoneNumber = ?pn,Email = ?em,Icon = ?ic,IsAdmin = ?isa where UserId = ?ui",
                new
                {
                    un = newUserInfo.UserName, jn = newUserInfo.JobNumber, up = newUserInfo.UserProfiles,
                    pn = newUserInfo.PhoneNumber, em = newUserInfo.Email, ic = newUserInfo.IconId,
                    isa=newUserInfo.Permissions.ToString(),
                    ui = newUserInfo.UserId
                });
            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                  "where DisplayName = ?un and JobNumber = ?jn and Description = ?up and PhoneNumber = ?pn and Email = ?em and Icon = ?ic and IsAdmin = ?isa and UserId = ?ui",
                new
                {
                    un = newUserInfo.UserName,
                    jn = newUserInfo.JobNumber,
                    up = newUserInfo.UserProfiles,
                    pn = newUserInfo.PhoneNumber,
                    em = newUserInfo.Email,
                    ic = newUserInfo.IconId,
                    isa = newUserInfo.Permissions.ToString(),
                    ui = newUserInfo.UserId
                });

            if (Target is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //TODO:函数功能：删除用户 输入：用户Id 输出：是否成功
        public async UnaryResult<bool> DeleteUser(ulong userId)
        {
            await Fsql.Ado.QueryAsync<object>("delete from user where UserId = ?ui", new {ui = userId});

            List<ulong> Target = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                  "where UserId = ?ui", new {ui = userId});
            if (Target is null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //TODO://函数功能：根据ID和注册信息注册新用户 输入：注册用户信息 输出：注册信息提示信息(-1表示该工号被注册，-2表示该手机号码被注册，0表示数据库因故障未插入成功,1表示成功)
        public async UnaryResult<int> RegisteredUsers(ulong userId, RegistrationInformation registrationInformations)
        {
            int mark = 0;

            List<ulong> Target1 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   "where JJobNumber = ?jn",
                new {jn = registrationInformations.JobNumber});


            List<ulong> Target2 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   "where PhoneNumber = ?pn",
                new {pn = registrationInformations.PhoneNumber});
            if (!(Target1 is null))
            {
                mark = -1;
            }
            else if (!(Target2 is null))
            {
                mark = -2;
            }
            else
            {
                mark = 1;
            }

            if (mark == 1)
            {
                await Fsql.Ado.QueryAsync<object>("Insert into user values(?ui,?un,?jn,null,?pn,null,1,?isa,?pw)",
                    new
                    {
                        ui = userId, un = registrationInformations.UserName, jn = registrationInformations.JobNumber,
                        pn = registrationInformations.PhoneNumber,
                        isa = registrationInformations.Permissions.ToString(), pw = "123456789"
                    });
            }

            List<ulong> Target3 = await Fsql.Ado.QueryAsync<ulong>("select UserId from user" +
                                                                   "where UserId = ?ui", new {ui = userId});
            if (Target3 is null)
            {
                mark = 0;
            }

            return mark;
        }
        //TODO:函数功能：审核消息 输入：用户Id与审核结果（0表示不通过，1表示通过）,根据审核结果修改用户数据 输出：是否成功
        public async UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：按照工号查找用户 输入：用户工号 输出：用户信息
        public async UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            List<(ulong, string, string, string, string, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string)>(
                    "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email from user" +
                    "where JobNumber=?jn", new {jn = jobNumber});
            if (!(Target is null))
            {
                var user = new UserInfo();
                var temp = Target[0];

                user.UserId = temp.Item1;
                user.UserName = temp.Item2;
                user.JobNumber = temp.Item3;
                user.UserProfiles = temp.Item4;
                user.PhoneNumber = temp.Item5;
                user.Email = temp.Item6;

                return user;
            }
            else
            {
                return null;
            }
        }
        //TODO:函数功能：按照用户Id查找用户 输入：用户Id 输出：用户信息
        public async UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            List<(ulong, string, string, string, string, string)> Target =
                await Fsql.Ado.QueryAsync<(ulong, string, string, string, string, string)>(
                    "select UserId,DisplayName,JobNumber,Description,PhoneNumber,Email from user" +
                    "where UserId = ?ui", new { ui=userId });
            if (!(Target is null))
            {
                var user = new UserInfo();
                var temp = Target[0];

                user.UserId = temp.Item1;
                user.UserName = temp.Item2;
                user.JobNumber = temp.Item3;
                user.UserProfiles = temp.Item4;
                user.PhoneNumber = temp.Item5;
                user.Email = temp.Item6;

                return user;
            }
            else
            {
                return null;
            }
        }
    }
}