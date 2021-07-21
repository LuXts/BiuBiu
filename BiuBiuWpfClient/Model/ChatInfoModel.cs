using System.Windows.Media.Imaging;
using HandyControl.Data;

namespace BiuBiuWpfClient.Model
{
    public enum TypeLocalMessageLocation
    {
        chatRecv,
        chatSend
    }

    public enum ChatInfoType
    {
        TextTypeChat,
        ImageTypeChat,
        FileTypeChat
    }

    public class ChatInfoModel
    {
        public object Message { get; set; }

        public string SenderId { get; set; }

        public TypeLocalMessageLocation Role { get; set; }

        public ChatInfoType Type { get; set; }

        public object Enclosure { get; set; }

        public string MessageOnwer { get; set; }

        public BitmapImage BImage { get; set; }
    }
}