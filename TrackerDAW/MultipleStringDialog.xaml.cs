using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for MultipleStringDialog.xaml
    /// </summary>
    public partial class MultipleStringDialog : Window
    {
        public string Value => this.valueTextBox.Text;

        public MultipleStringDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.valueTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {            
            this.DialogResult = true;
        }

        public static MultipleStringDialog Create(string title, string initialValue = "")
        {
            var dialog = new MultipleStringDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.Title = title;
            dialog.dialogTitleLabel.Content = title;
            dialog.valueTextBox.Text = initialValue;

            return dialog;
        }
    }
}
