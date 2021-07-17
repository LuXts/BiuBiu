using MessagePack;

namespace BiuBiuShare.Response
{
    [MessagePackObject(true)]
    public class MessageResponse
    {
        public ulong SourceId { get; set; }
        public ulong TargetId { get; set; }
        public ulong MessageId { get; set; }
        public bool Success { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }

        public static MessageResponse Failed { get; }
            = new MessageResponse() { Success = false };
    }
}