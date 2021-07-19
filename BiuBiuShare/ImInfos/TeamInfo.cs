using MessagePack;

namespace BiuBiuShare.ImInfos
{
    /// <summary>
    /// 群组信息类
    /// </summary>
    [MessagePackObject(true)]
    public class TeamInfo
    {
        public ulong TeamId { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public ulong OwnerId { get; set; }
        public ulong IconId { get; set; }
    }
}