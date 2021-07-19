using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class GroupInvitationResponse : GroupInvitation
    {
        public bool Success { get; set; }

        public static GroupInvitationResponse Failed { get; }
            = new GroupInvitationResponse() { Success = false };

        public GroupInvitationResponse()
        {
        }

        public GroupInvitationResponse(GroupInvitation invitation)
        {
            this.InvitationId = invitation.InvitationId;
            this.GroupId = invitation.GroupId;
            this.ReceiverId = invitation.ReceiverId;
            this.InvitationMessage = invitation.InvitationMessage;
            this.InvitationResult = invitation.InvitationResult;
        }
    }
}