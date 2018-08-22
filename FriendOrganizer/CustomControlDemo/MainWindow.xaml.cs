using System;
using System.ComponentModel;
using System.Windows;
using CustomControlLib;

namespace CustomControlDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataObject { Name = "Brian Lagunas" };

            DataObject obj1 = new DataObject();
            DataObject obj2 = new DataObject();

            l1.Items.Add(obj1);
            l2.Items.Add(obj2);

            //two ways of programatically setting the attached property in a control!
            //MyAttachedPropCustomControl.OnInitialized occurs before this code => the update of attached property won't work here!
            _stackPanel.SetValue(MyAttachedPropCustomControl.IncludeChildCountProperty, true);
            MyAttachedPropCustomControl.SetIncludeChildCount(_stackPanel, true);
        }

        public class DataObject : INotifyPropertyChanged
        {
            private String _name;
            public String Name
            {
                get => _name;
                set
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
