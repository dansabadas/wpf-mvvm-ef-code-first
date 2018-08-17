using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyContentControl : ContentControl
    {
        static MyContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyContentControl), new FrameworkPropertyMetadata(typeof(MyContentControl)));
        }
    }
}
