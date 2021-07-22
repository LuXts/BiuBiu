using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace BiuBiuWpfClient
{
    // 继承 INotifyPropertyChanged 这个接口
    public class Temp : INotifyPropertyChanged
    {
        // 必须实现这个事件
        public event PropertyChangedEventHandler PropertyChanged;

        // 这个函数不是必须的，但是可以简化开发，看不懂的可以直接先抄下来
        private void Notify(String propertyName)
        {
            PropertyChanged?.Invoke(this
                , new PropertyChangedEventArgs(propertyName));
        }

        // 私有的成员变量
        private int _number;

        // 公开的成员变量（或者是属性？我不清楚学名
        public int MyNumber
        {
            // get里面直接返回私有成员变量
            get { return _number; }
            // set最重要
            set
            {
                _number = value;
                // 要在这里通知 MyNumber 已经被修改了
                Notify("MyNumber");
            }
        }

        // 假设这是个窗口初始化的函数
        public void mian()
        {
            // 假设我们有个 ListBox 的对象
            ListBox listBox = new ListBox();

            // 创建一个观察者列表

            ObservableCollection<Temp> list = new ObservableCollection<Temp>();

            // 以上没有任何问题吧

            // 绑定数据（也可以在 xaml 文件绑定，随你喜欢）
            listBox.ItemsSource = list;

            // 调用 list 的 Add 接口能让 UI 直接更新

            list.Add(new Temp() { MyNumber = 2 });

            // 修改 list 里面的某个元素的 MyNumber 也可以让 UI 更新
            list[0].MyNumber = 100;
        }
    }
}