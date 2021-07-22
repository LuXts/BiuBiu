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
    /// IPSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IPSettingWindow : WindowX
    {
        public IPSettingWindow()
        {
            InitializeComponent();
            IpTextBox.Text = Initialization.LiteDb.GetIp();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Initialization.GrpcIp = IpTextBox.Text;
            Initialization.LiteDb.SetIp(IpTextBox.Text);
            Initialization.Init();
            this.Close();
        }
    }
}