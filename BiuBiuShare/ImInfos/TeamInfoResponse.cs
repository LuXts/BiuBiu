using MessagePack;

namespace BiuBiuShare.ImInfos
{
    [MessagePackObject(true)]
    public class TeamInfoResponse : TeamInfo
    {
        public bool Success { get; set; }

        public static TeamInfoResponse Failed { get; }
            = new TeamInfoResponse() { Success = false };

        public TeamInfoResponse()
        {
        }

        public TeamInfoResponse(TeamInfo teamInfo)
        {
            this.TeamId = teamInfo.TeamId;
            this.TeamName = teamInfo.TeamName;
            this.Description = teamInfo.Description;
            this.OwnerId = teamInfo.OwnerId;
            this.IconId = teamInfo.IconId;
        }
    }
}