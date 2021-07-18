using BiuBiuServer.Services;

namespace BiuBiuServer
{
    public class Initialization
    {
        public Initialization()
        {
            for (uint i = 0; i < 200; i++)
            {
                TalkService.PortList.AddLast(i);
            }
        }
    }
}