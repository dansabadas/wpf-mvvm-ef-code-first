using System;
using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyAttachedPropCustomControl : ContentControl
    {
        static MyAttachedPropCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyAttachedPropCustomControl), new FrameworkPropertyMetadata(typeof(MyAttachedPropCustomControl)));
        }

        public static readonly DependencyProperty ChildCountProperty =
            DependencyProperty.Register("ChildCount", typeof(int),
                typeof(MyAttachedPropCustomControl), new PropertyMetadata(0));

        public int ChildCount
        {
            get => (int)GetValue(ChildCountProperty);
            set => SetValue(ChildCountProperty, value);
        }

        public static readonly DependencyProperty IncludeChildCountProperty = 
            DependencyProperty.RegisterAttached("IncludeChildCount", typeof(bool),
                typeof(MyAttachedPropCustomControl), new PropertyMetadata(false));

        public static bool GetIncludeChildCount(DependencyObject obj)
        {
            return (bool)obj.GetValue(IncludeChildCountProperty);
        }

        public static void SetIncludeChildCount(DependencyObject obj, bool value)
        {
            obj.SetValue(IncludeChildCountProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (Content != null)
            {
                if (Content is Panel panel)
                {
                    if (GetIncludeChildCount(panel))
                    {
                        ChildCount++;
                    }

                    foreach (FrameworkElement child in panel.Children)
                    {
                        if (GetIncludeChildCount(child))
                        {
                            ChildCount++;
                        }
                    }
                }
                else
                {
                    if (GetIncludeChildCount(Content as DependencyObject))
                    {
                        ChildCount++;
                    }
                }
            }
        }
    }
}
