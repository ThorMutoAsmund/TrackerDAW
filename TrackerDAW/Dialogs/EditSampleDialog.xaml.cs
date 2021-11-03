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
    /// Interaction logic for EditSampleDialog.xaml
    /// </summary>
    public partial class EditSampleDialog : Window
    {
        private Sample sample;

        public EditSampleDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.partNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var partName = this.partNameTextBox.Text;

            if (!Env.TryParseDouble(this.partOffsetTextBox.Text, out var partOffset) || partOffset < 0d)
            {
                MessageBox.Show("Illegal part offset");
                return;
            }

            if (!Env.TryParseDouble(this.partGainTextBox.Text, out var partGain) || partGain < 0d)
            {
                MessageBox.Show("Illegal part gain");
                return;
            }

            this.sample.Name = partName;
            this.sample.Offset = partOffset;
            this.sample.Gain = partGain;
            this.DialogResult = true;
        }

        public static void ShowDialog(Sample sample)
        {
            var dialog = new EditSampleDialog()
            {
                Owner = Env.MainWindow,
                sample = sample
            };

            dialog.partNameLabel.Content = sample.Name;
            dialog.partNameTextBox.Text = sample.Name;
            dialog.partGainTextBox.Text = Env.GainToString(sample.Gain);
            dialog.partOffsetTextBox.Text = Env.TimeToString(sample.Offset);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPartChanged(sample);
            }
        }

        private void partNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.partNameLabel.Content = this.partNameTextBox.Text;
        }
    }
}
