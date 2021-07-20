using System;
using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.SignIn;
using MagicOnion;

namespace BiuBiuServer.Database
{
    /// <summary>
    /// 数据库驱动实现
    /// </summary>
    public class AccountDatabaseDriven : IAccountDatabaseDriven
    {
        private readonly IFreeSql Fsql = MySqlDriven.GetFreeSql();

        //输入 电话号码/工号 和 密码 输出登录结果，登录结果构造方式参考 AccountDatabaseDrivenTest 类
        public async UnaryResult<SignInResponse> CommonSign(string signInId, string password)
        {
            List<(string, ulong)> Target = await Fsql.Ado.QueryAsync<(string, ulong)>(
                "select DisplayName,UserId from user" +
                " where (PhoneNumber = ?si or JobNumber = ?si) and Password = ?pd ", new { si = signInId, pd = password });
            if (Target.Count != 0)
            {
                var VARIABLE = Target[0];
                var sign = new SignInResponse()
                {
                    DisplayName = VARIABLE.Item1,
                    UserId = VARIABLE.Item2,
                    Success = true
                };
                return sign;
            }
            else
            {
                return SignInResponse.Failed;
            }
        }

        //同上但是是管理员登录接口
        public async UnaryResult<SignInResponse> AdministrantSign(string signInId, string password)
        {
            List<(string, ulong)> Target = await Fsql.Ado.QueryAsync<(string, ulong)>(
                "select DisplayName,UserId from user" +
                " where (PhoneNumber = ?si or JobNumber = ?si) and IsAdmin = 'true' and Password = ?pd", new { si = signInId, pd = password });
            if (Target.Count != 0)
            {
                var VARIABLE = Target[0];
                Console.WriteLine(VARIABLE.Item1);
                Console.WriteLine(VARIABLE.Item2);
                var sign = new SignInResponse()
                {
                    DisplayName = VARIABLE.Item1,
                    UserId = VARIABLE.Item2,
                    Success = true
                };
                return sign;
            }
            else
            {
                return SignInResponse.Failed;
            }
        }
    }
}