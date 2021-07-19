using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class TeamRequestResponse : TeamRequest
    {
        public bool Success { get; set; }

        public static TeamRequestResponse Failed { get; }
            = new TeamRequestResponse() { Success = false };

        public TeamRequestResponse()
        {
        }

        public TeamRequestResponse(TeamRequest request)
        {
            this.RequestId = request.RequestId;
            this.SenderId = request.SenderId;
            this.TeamId = request.TeamId;
            this.RequestMessage = request.RequestMessage;
            this.RequestResult = request.RequestResult;
        }
    }
}