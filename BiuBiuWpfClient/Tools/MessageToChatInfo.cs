using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.Tool;
using BiuBiuWpfClient.Login;
using BiuBiuWpfClient.Model;

namespace BiuBiuWpfClient.Tools
{
    public class MessageToChatInfo
    {
        public static async Task<ChatInfoModel> TransformChatInfoModel(
            Message message)
        {
            object data;
            ChatInfoType type;
            if (message.Type == "Text")
            {
                data = message.Data;
                type = ChatInfoType.TextTypeChat;
            }
            else if (message.Type == "Image")
            {
                data = await Initialization.DataDb.GetBitmapImage(
                    message.MessageId);
                type = ChatInfoType.ImageTypeChat;
            }
            else
            {
                data = message.Data;
                type = ChatInfoType.FileTypeChat;
            }

            TypeLocalMessageLocation temp;
            if (message.SourceId == AuthenticationTokenStorage.UserId)
            {
                temp = TypeLocalMessageLocation.chatSend;
            }
            else
            {
                temp = TypeLocalMessageLocation.chatRecv;
            }

            UserInfo user
                = await Initialization.DataDb.GetUserInfoByServer(message.SourceId);

            var info = new ChatInfoModel
            {
                Message = data
                ,
                SenderId = message.SourceId.ToString()
                ,
                Type = type
                ,
                Role = temp
                ,
                MessageId = message.MessageId
                ,
                BImage = await Initialization.DataDb.GetBitmapImage(user.IconId)
                ,
                MessageOnwer = user.DisplayName
            };

            return info;
        }
    }
}