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
    /// Interaction logic for EditPatternDialog.xaml
    /// </summary>
    public partial class EditPartDialog : Window
    {
        private Part part;

        public EditPartDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.partNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var partName = this.partNameTextBox.Text;

            this.part.Name = partName;

            this.DialogResult = true;
        }

        public static EditPartDialog Create(Part part)
        {
            var dialog = new EditPartDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.part = part;
            dialog.partNameLabel.Content = part.Name;
            dialog.partNameTextBox.Text = part.Name;
            //dialog.patternLengthTextBox.Text = $"{pattern.Length}";
            //dialog.patternBPSTextBox.Text = $"{pattern.BPS}";

            return dialog;
        }

        private void partNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.partNameLabel.Content = this.partNameTextBox.Text;
        }
    }
}
