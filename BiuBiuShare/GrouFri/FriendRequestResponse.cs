using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class FriendRequestResponse : FriendRequest
    {
        public bool Success { get; set; }

        public static FriendRequestResponse Failed { get; }
            = new FriendRequestResponse() { Success = false };

        public FriendRequestResponse()
        {
        }

        public FriendRequestResponse(FriendRequest request)
        {
            this.RequestId = request.RequestId;
            this.SenderId = request.SenderId;
            this.ReceiverId = request.ReceiverId;
            this.RequestMessage = request.RequestMessage;
            this.RequestResult = request.RequestResult;
        }
    }
}