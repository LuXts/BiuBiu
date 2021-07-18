using BiuBiuServer.Services;

namespace BiuBiuServer
{
    public class Initialization
    {
        public readonly static string GrpcAddress = "https://localhost:5001";

        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Initialization()
        {
            for (uint i = 0; i < 200; i++)
            {
                TalkService.PortList.AddLast(i);
            }
        }
    }
}