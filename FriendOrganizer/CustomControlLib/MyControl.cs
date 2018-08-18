using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControlLib
{
    [TemplatePart(Name = TextBlockPart, Type = typeof(TextBlock))]  // this is more of a contract for third-party clients of our control. Not used programatically
    public class MyControl : Control
    {
        private const string TextBlockPart = "PART_TextBlock";
        
        TextBlock _textblock;
        protected TextBlock TextBlock
        {
            get => _textblock;
            set
            {
                if (_textblock != null)
                {
                    _textblock.TextInput -= textblock_TextInput;
                }

                _textblock = value;

                if (_textblock != null)
                {
                    _textblock.Text = "Set from code";
                    _textblock.TextInput += new TextCompositionEventHandler(textblock_TextInput);
                }
            }

        }
        static MyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyControl), new FrameworkPropertyMetadata(typeof(MyControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //var textBlock = GetTemplateChild(TextBlockPart) as TextBlock;
            //textBlock.Text = "Set from Code";

            TextBlock = GetTemplateChild(TextBlockPart) as TextBlock;
            TextBlock.Text = "Set from Code";
        }

        void textblock_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
