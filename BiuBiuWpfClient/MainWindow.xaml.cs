using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiuBiuWpfClient.Model;
using HandyControl.Tools;
using HandyControl.Data;

using HandyControl.Tools;

using Window = HandyControl.Controls.Window;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// [TemplatePart(Name = ElementNonClientArea, Type = typeof(UIElement))]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConfigHelper.Instance.SetWindowDefaultStyle();
            InitializeComponent();

            var list = new ObservableCollection<MainChatViewModel>();
            var view = CollectionViewSource.GetDefaultView(list);
            view.SortDescriptions.Add(new SortDescription("LastMessageTimeString"
                , ListSortDirection.Descending));
            list.Add(new MainChatViewModel() { LastMessageTimeString = "0" });
            list.Add(new MainChatViewModel() { LastMessageTimeString = "3" });
            list.Add(new MainChatViewModel() { LastMessageTimeString = "1" });
            ChatListBox.ItemsSource = view;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MiniButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private Rect rcnormal; //定义一个全局rect记录还原状态下窗口的位置和大小。

        /// <summary>
        /// 最大化
        /// </summary>
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
            rcnormal
                = new Rect(this.Left, this.Top, this.Width
                    , this.Height); //保存下当前位置与大小
            this.Left = 0; //设置位置
            this.Top = 0;
            Rect rc = SystemParameters.WorkArea; //获取工作区大小
            this.Width = rc.Width;
            this.Height = rc.Height;
            this.ResizeMode = ResizeMode.NoResize;
        }

        /// <summary>
        /// 还原
        /// </summary>
        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
            this.Left = rcnormal.Left;
            this.Top = rcnormal.Top;
            this.Width = rcnormal.Width;
            this.Height = rcnormal.Height;
            this.ResizeMode = ResizeMode.CanResize;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight > SystemParameters.WorkArea.Height ||
                this.ActualWidth > SystemParameters.WorkArea.Width)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                btnMaximize_Click(null, null);
            }

            RowDefinition.MaxHeight = this.Height / 2;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.ActualWidth == SystemParameters.WorkArea.Width)
                {
                    btnNormal_Click(null, null);
                }
                else
                {
                    btnMaximize_Click(null, null);
                }
            }
        }
    }
}