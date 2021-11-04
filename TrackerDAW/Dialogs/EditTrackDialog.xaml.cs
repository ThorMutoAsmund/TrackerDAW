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
    /// Interaction logic for EditTrackDialog.xaml
    /// </summary>
    public partial class EditTrackDialog : Window
    {
        private Track track;

        public EditTrackDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.trackGainTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var providerInfo = this.providerSelectorControl.ProviderInfo;

            if (!Env.TryParseDouble(this.trackGainTextBox.Text, out var trackGain) || trackGain < 0d)
            {
                MessageBox.Show("Illegal track gain");
                return;
            }

            this.track.Gain = trackGain;
            this.track.ProviderInfo = providerInfo;

            this.DialogResult = true;
        }

        public static void ShowDialog(Track track, string title)
        {
            var dialog = new EditTrackDialog()
            {
                Owner = Env.MainWindow,
                track = track
            };

            dialog.trackNameLabel.Content = title;
            dialog.trackGainTextBox.Text = Env.GainToString(track.Gain);
            dialog.providerSelectorControl.ProviderInfo = track.ProviderInfo;

            if (dialog.ShowDialog() == true)
            {
                Song.OnTrackChanged(track);
            }
        }
    }
}
