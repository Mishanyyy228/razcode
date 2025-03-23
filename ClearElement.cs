using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace lab
{
    public class ClearElement
    {
        public void ClearText(Label label, TextBlock textBlock, TextBlock textBlock1, TextBlock textBlock2)
        {
            label.Content = string.Empty;
            textBlock.Text = string.Empty;
            textBlock1.Text = string.Empty;
            textBlock2.Text = string.Empty;
        }
        public void HiddenElement(Button button,Button button1,ListBox ?listBox,ListBox ?listBox1)
        {
            button.Visibility = System.Windows.Visibility.Hidden;
            button1.Visibility = System.Windows.Visibility.Hidden;
            if (listBox != null)
            {
                listBox.Visibility = System.Windows.Visibility.Hidden;
            }
            if (listBox1 != null)
            {
                listBox1.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
