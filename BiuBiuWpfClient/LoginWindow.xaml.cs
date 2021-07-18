using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using BiuBiuWpfClient.Model;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel login = new LoginViewModel();

        private int max = 0;

        private readonly DispatcherTimer _timer;

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = login;
            login.Progress = 0;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            _timer.Tick += Timer_Tick;
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            login.Progress += 10;
            if (login.Progress == 100)
            {
                login.Progress = 0;
                _timer.Stop();
                login.IsUploading = false;
            }
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                login.IsUploading = false;
                _timer.Stop();
            }
            else
            {
                login.IsUploading = true;
                _timer.Start();
            }
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            max += 10;
        }
    }
}