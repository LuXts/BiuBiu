using MongoDB.Driver;

namespace BiuBiuServer
{
    public class MySqlDriven
    {
        private readonly static string _ip = "192.168.100.7";
        private readonly static string _user = "luxts";
        private readonly static string _password = "JL4i7nVuymwBnGr";

        private readonly static string _connectString =
            $"Data Source={_ip};Port=3306;User ID={_user};Password={_password};Initial Catalog=Test;Charset=utf8mb4;SslMode=none;Min pool size=1";

        private static IFreeSql _freeSql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, _connectString)
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
            .Build();

        public static IFreeSql GetFreeSql()
        {
            return _freeSql;
        }

        private readonly static string _noSqlConnectStr = "mongodb://127.0.0.1:27017";

        private static MongoClient _noSqlClient = new MongoClient(_noSqlConnectStr);

        public static MongoClient GetNoSqlClient()
        {
            return _noSqlClient;
        }
    }
}