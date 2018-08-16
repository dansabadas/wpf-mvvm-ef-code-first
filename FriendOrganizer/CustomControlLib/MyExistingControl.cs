using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyExistingControl : Control
    {
        static MyExistingControl()
        {
            // https://stackoverflow.com/questions/33049083/how-to-find-the-wpf-custom-control-template-in-add-new-item-dialog
            // the answer for converting normal class project type  into WPF Custom control project type
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyExistingControl), new FrameworkPropertyMetadata(typeof(MyExistingControl)));
        }
    }
}
