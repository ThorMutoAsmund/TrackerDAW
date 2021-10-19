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

            if (!Env.TryParseDouble(this.patternLengthTextBox.Text, out var patternLength) || patternLength < 0d)
            {
                MessageBox.Show("Illegal pattern length");
                return;
            }
            if (!Env.TryParseDouble(this.patternBPSTextBox.Text, out var patternBPS) || patternBPS < 0d)
            {
                MessageBox.Show("Illegal pattern BPS");
                return;
            }
            if (!Env.TryParseDouble(this.patternGainTextBox.Text, out var patternGain) || patternGain < 0d)
            {
                MessageBox.Show("Illegal pattern gain");
                return;
            }

            this.pattern.Name = patternName;
            this.pattern.Length = patternLength;
            this.pattern.BPS = patternBPS;
            this.pattern.Gain = patternGain;

            this.DialogResult = true;
        }



        public static void ShowDialog(Pattern pattern)
        {
            var dialog = new EditPatternDialog()
            {
                Owner = Env.MainWindow,
                pattern = pattern,
            };

            dialog.patternNameLabel.Content = pattern.Name;
            dialog.patternNameTextBox.Text = pattern.Name;
            dialog.patternLengthTextBox.Text = Env.TimeToString(pattern.Length);
            dialog.patternBPSTextBox.Text = Env.BPSToString(pattern.BPS);
            dialog.patternGainTextBox.Text = Env.GainToString(pattern.Gain);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPatternChanged(pattern);
            }
        }

        private void patternNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.patternNameLabel.Content = this.patternNameTextBox.Text;
        }
    }
}
