using System.Collections.ObjectModel;

namespace BiuBiuWpfClient.Model
{
    public class InfoViewModel
    {
        public static ObservableCollection<InfoListItem> FriendCollection
            = new ObservableCollection<InfoListItem>();

        public static ObservableCollection<InfoListItem> TeamCollection
            = new ObservableCollection<InfoListItem>();

        public static ObservableCollection<InfoListItem> NewFriendCollection
            = new ObservableCollection<InfoListItem>();

        public static ObservableCollection<InfoListItem> TeamInvitationCollection
            = new ObservableCollection<InfoListItem>();

        public static ObservableCollection<InfoListItem> TeamRequestCollection
            = new ObservableCollection<InfoListItem>();
    }
}