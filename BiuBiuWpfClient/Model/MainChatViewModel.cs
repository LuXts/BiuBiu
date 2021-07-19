using System;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Windows.Media.Imaging;

namespace BiuBiuWpfClient.Model
{
    public class MainChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private BitmapImage _bitmap;

        public BitmapImage BImage
        {
            get { return _bitmap; }
            set
            {
                _bitmap = value;
                Notify("Bimage");
            }
        }

        public string DisplayName;

        private ulong LastMessageTime;

        public string LastMessageTimeString
        {
            get { return LastMessageTime.ToString(); }
            set
            {
                LastMessageTime = Convert.ToUInt64(value);
                Notify("LastMessageTimeString");
            }
        }

        public MainChatViewModel()
        {
            _bitmap = new BitmapImage();
            _bitmap.BeginInit();
            _bitmap.StreamSource = new MemoryStream(File.ReadAllBytes("C://Users/LuXts/source/repos/BiuBiu/BiuBiuWpfClient/Resources/Image/ic_people_0076f6.png"));
            _bitmap.EndInit();
        }
    }
}