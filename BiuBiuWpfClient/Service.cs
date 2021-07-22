using BiuBiuShare.ServiceInterfaces;
using MagicOnion.Client;

namespace BiuBiuWpfClient
{
    public class Service
    {
        public static ITalkService TalkService;
        public static IImInfoService ImInfoService;
        public static IGroFriService GroFriService;
        public static IAdminService AdminService ;

        public static void InitService()
        {
            TalkService = MagicOnionClient.Create<ITalkService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            ImInfoService = MagicOnionClient.Create<IImInfoService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            GroFriService = MagicOnionClient.Create<IGroFriService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });
            AdminService = MagicOnionClient.Create<IAdminService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });
        }
    }
}