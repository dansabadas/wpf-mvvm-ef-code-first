using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyItemsControl : ItemsControl
    {
        static MyItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItemsControl), new FrameworkPropertyMetadata(typeof(MyItemsControl)));
        }
    }
}
