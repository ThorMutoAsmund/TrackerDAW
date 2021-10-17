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
    public partial class EditPatternDialog : Window
    {
        private Pattern pattern;

        public EditPatternDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.patternNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var patternName = this.patternNameTextBox.Text;

            if (!double.TryParse(this.patternLengthTextBox.Text, out var patternLength))
            {
                MessageBox.Show("Illegal pattern length");
                return;
            }
            if (!double.TryParse(this.patternBPSTextBox.Text, out var patternBPS))
            {
                MessageBox.Show("Illegal pattern BPS");
                return;
            }

            this.pattern.Name = patternName;
            this.pattern.Length = patternLength;
            this.pattern.BPS = patternBPS;                 

            this.DialogResult = true;
        }

        public static EditPatternDialog Create(Pattern pattern)
        {
            var dialog = new EditPatternDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.pattern = pattern;
            dialog.patternNameLabel.Content = pattern.Name;
            dialog.patternNameTextBox.Text = pattern.Name;
            dialog.patternLengthTextBox.Text = $"{pattern.Length}";
            dialog.patternBPSTextBox.Text = $"{pattern.BPS}";

            return dialog;
        }

        private void patternNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.patternNameLabel.Content = this.patternNameTextBox.Text;
        }
    }
}
