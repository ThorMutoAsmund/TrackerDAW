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
    /// Interaction logic for EditCompositionDialog.xaml
    /// </summary>
    public partial class EditCompositionDialog : Window
    {
        private Composition composition;

        public EditCompositionDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.partNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var partName = this.partNameTextBox.Text;
            var providerInfo = this.providerSelectorControl.ProviderInfo;

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

            if (!Env.TryParseDouble(this.compositionLengthTextBox.Text, out var compositionLength) || compositionLength < 0d)
            {
                MessageBox.Show("Illegal composition length");
                return;
            }

            this.composition.Name = partName;
            this.composition.Offset = partOffset;
            this.composition.Gain = partGain;
            this.composition.Length = compositionLength;
            this.composition.ProviderInfo = providerInfo;

            this.DialogResult = true;
        }

        public static void ShowDialog(Composition composition)
        {
            var dialog = new EditCompositionDialog()
            {
                Owner = Env.MainWindow,
                composition = composition
            };

            dialog.partNameLabel.Content = composition.Name;
            dialog.partNameTextBox.Text = composition.Name;
            dialog.partGainTextBox.Text = Env.GainToString(composition.Gain);
            dialog.partOffsetTextBox.Text = Env.TimeToString(composition.Offset);
            dialog.compositionLengthTextBox.Text = Env.TimeToString(composition.Length);
            dialog.providerSelectorControl.ProviderInfo = composition.ProviderInfo;

            if (dialog.ShowDialog() == true)
            {
                Song.OnPartChanged(composition);
            }
        }

        private void partNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.partNameLabel.Content = this.partNameTextBox.Text;
        }
    }
}
