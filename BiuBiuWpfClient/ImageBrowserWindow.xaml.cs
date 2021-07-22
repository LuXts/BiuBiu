using Panuon.UI.Silver;
using System.Windows.Media.Imaging;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// ImageBrowserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageBrowserWindow : WindowX
    {
        public ImageBrowserWindow()
        {
            InitializeComponent();
        }

        public void InitWindow(BitmapImage bitmap, string title)
        {
            ImageBViewer.ImageSource = BitmapFrame.Create(bitmap);
            this.Title = title;
        }
    }
}