using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyCustomControl : Control
    {
        static MyCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCustomControl), new FrameworkPropertyMetadata(typeof(MyCustomControl)));
        }

        public MyCustomControl()
        {

        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyCustomControl));   // set in a style, supports data binding, animation, set with a Resource

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(MyCustomControl),
                new FrameworkPropertyMetadata("Default",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTextPropertyChanged),
                    new CoerceValueCallback(OnTextPropertyCoerce)));

        public string Text2
        {
            get => (string)GetValue(Text2Property);
            set => SetValue(Text2Property, value);
        }

        private static object OnTextPropertyCoerce(DependencyObject d, object baseValue)
        {
            // allows to change the incoming value = baseValue
            // it only updates the coontrol, not the underlying value
            // this fires first, before OnTextPropertyChanged
            if ((string) baseValue == "Dan")
            {
                return "Changed " + baseValue;
            }

            return baseValue;
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyCustomControl control)
                control.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnTextPropertyChanged(string oldValue, string newValue)
        {
            Debug.WriteLine($"Debugging {oldValue}->{newValue}");
        }
    }
}
