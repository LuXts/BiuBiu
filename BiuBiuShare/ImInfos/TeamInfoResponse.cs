using MessagePack;

namespace BiuBiuShare.ImInfos
{
    [MessagePackObject(true)]
    public class TeamInfoResponse : TeamInfo
    {
        public bool Success { get; set; }

        public static TeamInfoResponse Failed { get; }
            = new TeamInfoResponse() { Success = false };
    }
}