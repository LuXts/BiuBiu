using System.Windows;
using System.Windows.Controls;

namespace BiuBiuWpfClient.Model
{
    internal class ChatBubbleSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var u = container as FrameworkElement;

            ChatInfoModel message = item as ChatInfoModel;

            if (message.Role == TypeLocalMessageLocation.chatSend)
                return u.FindResource("chatSend") as DataTemplate;
            else
                return u.FindResource("chatRecv") as DataTemplate;
        }
    }
}