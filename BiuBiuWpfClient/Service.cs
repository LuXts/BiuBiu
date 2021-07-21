using BiuBiuServer.Userhub;
using BiuBiuShare.ServiceInterfaces;
using MagicOnion.Client;

namespace BiuBiuWpfClient
{
    public class Service
    {
        public static ITalkService TalkService;
        public static IImInfoService ImInfoService;

        public static void InitService()
        {
            TalkService = MagicOnionClient.Create<ITalkService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            ImInfoService = MagicOnionClient.Create<IImInfoService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });
        }
    }
}