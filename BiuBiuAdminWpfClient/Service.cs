using BiuBiuServer;
using BiuBiuShare.ServiceInterfaces;
using MagicOnion.Client;

namespace BiuBiuAdminWpfClient
{
    public class Service
    {
        public static IAdminService AdminService;
        public static IImInfoService ImInfoService;

        public static void InitService()
        {
            AdminService = MagicOnionClient.Create<IAdminService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            ImInfoService = MagicOnionClient.Create<IImInfoService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });
        }
    }
}