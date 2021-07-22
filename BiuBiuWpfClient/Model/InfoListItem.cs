using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BiuBiuWpfClient.Model
{
    public class InfoListItem : INotifyPropertyChanged
    {
        public enum InfoType
        {
            Friend,
            Team,
            NewFriend,
            TeamInvitation,
            TeamRequest
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            PropertyChanged?.Invoke(this
                , new PropertyChangedEventArgs(propertyName));
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                Notify("DisplayName");
            }
        }

        private BitmapImage _bitmap;

        public BitmapImage BImage
        {
            get { return _bitmap; }
            set
            {
                _bitmap = value;
                Notify("BImage");
            }
        }

        public ulong InfoId;

        public InfoType Type;
    }
}