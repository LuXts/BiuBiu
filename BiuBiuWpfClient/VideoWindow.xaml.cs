using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BiuBiuShare.TalkInfo;
using BiuBiuWpfClient.Login;
using HandyControl.Tools;
using LibVLCSharp.Shared;
using Xabe.FFmpeg;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// VideoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VideoWindow : Window
    {
        private readonly LibVLC _libvlc;

        public VideoWindow()
        {
            InitializeComponent();

            Core.Initialize();
            _libvlc = new LibVLC();
            this.Closed += Window_OnClose;
        }

        private CancellationTokenSource cancellationTokenSource;

        private ulong _targetId;

        public async void Send(string path, ulong targetId)
        {
            _targetId = targetId;
            cancellationTokenSource = new CancellationTokenSource();
            FFmpeg.Conversions.New()
                .AddDesktopStream()
                .AddParameter("-f rtsp -rtsp_transport tcp " + path)
                .Start(cancellationTokenSource.Token);
        }

        public void Play(string path)
        {
            MyVideoView.MediaPlayer = new MediaPlayer(_libvlc);
            var m = new Media(_libvlc, path, FromType.FromLocation);
            //一下是设置相应的参数，播放rtsp流的生活，设置和不设置是有很大差距的，大家可以注释代码体验一下
            m.AddOption(":rtsp-tcp");
            m.AddOption(":clock-synchro=0");
            m.AddOption(":live-caching=0");
            m.AddOption(":network-caching=333");
            m.AddOption(":file-caching=0");
            m.AddOption(":grayscale");
            MyVideoView.MediaPlayer.Play(m);
        }

        public void Window_OnClose(object sender, System.EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            MyVideoView?.Dispose();
            Out();
        }

        private async void Out()
        {
            await Service.TalkService.SendMessageAsync(new Message()
            {
                Data = "Out"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = _targetId
                ,
                Type = "Video"
            });
        }
    }
}