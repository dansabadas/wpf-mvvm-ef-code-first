using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class ReadonlyPropControl : Control
    {
        static ReadonlyPropControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReadonlyPropControl), new FrameworkPropertyMetadata(typeof(ReadonlyPropControl)));
        }
    }
}
