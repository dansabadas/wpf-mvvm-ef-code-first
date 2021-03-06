﻿using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class MyTextControl : Control
    {
        static MyTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyTextControl), new FrameworkPropertyMetadata(typeof(MyTextControl)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyTextControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// This is just a helper to access the DP => do not put logic into the getter and setter.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
