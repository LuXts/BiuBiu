using MessagePack;

namespace BiuBiuShare.TalkInfo
{
    [MessagePackObject(true)]
    public class Message
    {
        public ulong SourceId { get; set; }
        public ulong TargetId { get; set; }
        public ulong MessageId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}