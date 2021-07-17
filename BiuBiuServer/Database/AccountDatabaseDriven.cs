using System;
using BiuBiuServer.Interfaces;
using BiuBiuShare.Response;
using FreeSql.Sqlite;
using MagicOnion;

namespace BiuBiuServer.Database
{
    
    public class AccountDatabaseDriven : IAccountDatabaseDriven
    {


        IFreeSql Fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=db1.db")
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
            .Build(); //请务必定义成 Singleton 单例模式

        //注意： IFreeSql 在项目中应以单例声明，而不是在每次使用的时候创建。

        // TODO: 输入 电话号码/工号 和 密码 输出登录结果，登录结果构造方式参考 AccountDatabaseDrivenTest 类
        public async UnaryResult<SignInResponse> CommonSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
            var Target = Fsql.Select<T>()
        }

        // TODO：同上但是是管理员登录接口
        public async UnaryResult<SignInResponse> AdministrantSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}