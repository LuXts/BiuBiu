using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class GroupRequest
    {
        public ulong InvitationId { get; set; }
        public ulong SenderId { get; set; }//申请者ID
        public ulong GroupId { get; set; }
        public string InvitationMessage { get; set; }
        public string InvitationResult { get; set; }
    }
}