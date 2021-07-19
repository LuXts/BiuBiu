using MessagePack;

namespace BiuBiuShare.TalkInfo
{
    [MessagePackObject(true)]
    public class MessageResponse : Message
    {
        public bool Success { get; set; }

        public static MessageResponse Failed { get; }
            = new MessageResponse() { Success = false };

        public MessageResponse()
        {
        }

        public MessageResponse(Message message)
        {
            this.SourceId = message.SourceId;
            this.TargetId = message.TargetId;
            this.MessageId = message.MessageId;
            this.Type = message.Type;
            this.Data = message.Data;
        }
    }
}