using BiuBiuShare.ServiceInterfaces;
using MagicOnion.Client;
using System.Timers;

namespace BiuBiuWpfClient
{
    public class Service
    {
        public static ITalkService TalkService;
        public static IImInfoService ImInfoService;
        public static IGroFriService GroFriService;
        public static IAdminService AdminService;

        private static IKeepAliveService _keepAliveService;

        private static Timer _timer;

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

            _keepAliveService = MagicOnionClient.Create<IKeepAliveService>(
                Initialization.GChannel, new[] { Initialization.ClientFilter });

            _timer = new Timer(30000);
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var temp = _keepAliveService.SendHeartbeatPacket();
        }
    }
}