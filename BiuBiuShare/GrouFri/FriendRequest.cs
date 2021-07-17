using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class FriendRequest
    {
        public ulong RequestId { get; set; }
        public ulong SenderId { get; set; }//邀请者ID
        public ulong ReceiverId { get; set; }//被邀请者ID
        public string InvitationMessage { get; set; }
        public string InvitationResult { get; set; }
    }

}