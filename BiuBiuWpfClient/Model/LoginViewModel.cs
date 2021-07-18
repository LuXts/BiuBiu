using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BiuBiuWpfClient.Annotations;

namespace BiuBiuWpfClient.Model
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                Notify("Progress");
            }
        }

        private bool _isUploading;

        public bool IsUploading
        {
            get { return _isUploading; }
            set
            {
                _isUploading = value;
                Notify("IsUploading");
            }
        }
    }
}