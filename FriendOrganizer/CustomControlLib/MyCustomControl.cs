﻿using System.Collections;
using System.Collections.ObjectModel;
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
            Items = new ObservableCollection<object>();
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

        // below logic for read-only props (RegisterReadOnly)
        private static readonly DependencyPropertyKey HasBeenClickedPropertyKey =
            DependencyProperty.RegisterReadOnly("HasBeenClicked", typeof(bool), typeof(MyCustomControl), new PropertyMetadata(false));

        public static readonly DependencyProperty HasBeenClickedProperty = HasBeenClickedPropertyKey.DependencyProperty;

        public bool HasBeenClicked => (bool)GetValue(HasBeenClickedProperty);   // read-only => it has only a getter

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //demo purposes only, check for previous instances 
            //and remove handler first
            if (GetTemplateChild("PART_Button") is Button button)
                button.Click += Button_Click;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetValue(HasBeenClickedPropertyKey, true);
        }

        public static readonly DependencyProperty ItemsProperty =
            // DependencyProperty.Register("Items", typeof(IList), typeof(MyCustomControl), new PropertyMetadata(new ObservableCollection<object>())); // this way, specifying a default value in prop metadata, we inadvertently created a singleton!
            DependencyProperty.Register("Items", typeof(IList), typeof(MyCustomControl));

        public IList Items
        {
            get => (IList)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
    }
}
