using Panuon.UI.Silver;
using System.Windows;

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