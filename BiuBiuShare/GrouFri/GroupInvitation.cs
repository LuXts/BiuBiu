using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)] 
    public class GroupInvitation
    {
        public ulong InvitationId { get; set; }
        public ulong ReceiverId { get; set; }//被邀请者ID
        public ulong GroupId { get; set; }
        public string InvitationMessage { get; set; }
        public string InvitationResult { get; set; }
    }
}