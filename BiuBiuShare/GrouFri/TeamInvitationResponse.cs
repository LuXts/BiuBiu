using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class TeamInvitationResponse : TeamInvitation
    {
        public bool Success { get; set; }

        public static TeamInvitationResponse Failed { get; }
            = new TeamInvitationResponse() { Success = false };

        public TeamInvitationResponse()
        {
        }

        public TeamInvitationResponse(TeamInvitation invitation)
        {
            this.InvitationId = invitation.InvitationId;
            this.TeamId = invitation.TeamId;
            this.ReceiverId = invitation.ReceiverId;
            this.InvitationMessage = invitation.InvitationMessage;
            this.InvitationResult = invitation.InvitationResult;
        }
    }
}