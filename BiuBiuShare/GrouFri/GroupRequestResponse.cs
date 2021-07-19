using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class GroupRequestResponse : GroupRequest
    {
        public bool Success { get; set; }

        public static GroupRequestResponse Failed { get; }
            = new GroupRequestResponse() { Success = false };

        public GroupRequestResponse()
        {
        }

        public GroupRequestResponse(GroupRequest request)
        {
            this.RequestId = request.RequestId;
            this.SenderId = request.SenderId;
            this.GroupId = request.GroupId;
            this.RequestMessage = request.RequestMessage;
            this.RequestResult = request.RequestResult;
        }
    }
}