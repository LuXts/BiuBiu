using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Panuon.UI.Silver;

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