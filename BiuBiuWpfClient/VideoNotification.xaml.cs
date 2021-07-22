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
using BiuBiuShare.TalkInfo;
using BiuBiuWpfClient.Login;
using Panuon.UI.Silver;

namespace BiuBiuWpfClient
{
    /// <summary>
    /// VideoNotification.xaml 的交互逻辑
    /// </summary>
    public partial class VideoNotification : WindowX
    {
        private bool _mark;

        private ulong _targetId;

        public bool D;

        public VideoNotification()
        {
            InitializeComponent();
            this.Closed += Window_Closed;
        }

        private async void Window_Closed(object sender, System.EventArgs e)
        {
            if (D)
            {
                if (_mark)
                {
                    await Service.TalkService.SendMessageAsync(new Message()
                    {
                        Data = "No"
                        ,
                        SourceId = AuthenticationTokenStorage.UserId
                        ,
                        TargetId = _targetId
                        ,
                        Type = "Video"
                    });
                }
                else
                {
                    await Service.TalkService.SendMessageAsync(new Message()
                    {
                        Data = "Cancel"
                        ,
                        SourceId = AuthenticationTokenStorage.UserId
                        ,
                        TargetId = _targetId
                        ,
                        Type = "Video"
                    });
                }
            }

            if (DialogResult is null)
            {
                DialogResult = false;
            }
        }

        public void Init(string data, bool mark, ulong targetId)
        {
            D = true;
            DataBlock.Text = data;
            if (mark)
            {
                OkButton.Visibility = Visibility.Hidden;
                NoButton.Visibility = Visibility.Hidden;
            }
            else
            {
                CancelButton.Visibility = Visibility.Hidden;
            }
            _mark = mark;
        }

        private async void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Service.TalkService.SendMessageAsync(new Message()
            {
                Data = "Ok"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = _targetId
                ,
                Type = "Video"
            });
            D = false;
            DialogResult = true;
            this.Close();
        }

        private async void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Service.TalkService.SendMessageAsync(new Message()
            {
                Data = "No"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = _targetId
                ,
                Type = "Video"
            });
            D = false;
            DialogResult = false;
            this.Close();
        }

        private async void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Service.TalkService.SendMessageAsync(new Message()
            {
                Data = "Cancel"
                ,
                SourceId = AuthenticationTokenStorage.UserId
                ,
                TargetId = _targetId
                ,
                Type = "Video"
            });
            D = false;
            this.Close();
        }
    }
}