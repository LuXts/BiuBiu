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

        // 约定 TeamId = 0 代表没有这个群组
        public static TeamInfo NullTeam = new TeamInfo() { TeamId = 0 };

        public static bool operator ==(TeamInfo lhs, TeamInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TeamInfo lhs, TeamInfo rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}