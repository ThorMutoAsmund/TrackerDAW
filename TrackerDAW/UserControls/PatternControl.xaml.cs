using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for TrackUserControl.xaml
    /// </summary>
    public partial class PatternControl : UserControl
    {
        private Pattern pattern;
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private Thread playPositionThread;

        public PatternControl()
        {
            InitializeComponent();

            Env.SelectedPatternChanged += Env_SelectedPatternChanged;
            Song.PatternChanged += Song_PatternChanged;

            Audio.WaveOut.PlaybackStopped += Audio_PlaybackStopped;

            this.playPositionThread = new Thread(CheckPlayPosition);
            this.playPositionThread.Start();
            Env.ApplicationEnded += Env_ApplicationEnded;
        }


        private void Env_ApplicationEnded()
        {
            this.playPositionThread.Abort();
        }

        private void CheckPlayPosition()
        {
            for (; ; )
            {
                if (Audio.WaveOut.PlaybackState == PlaybackState.Playing)
                {
                    double ms = Audio.WaveOut.GetPosition() * 1000.0 / Audio.WaveOut.OutputWaveFormat.BitsPerSample / Audio.WaveOut.OutputWaveFormat.Channels * 8 / Audio.WaveOut.OutputWaveFormat.SampleRate;

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Audio_PlayPositionChanged(ms);
                    }));                    
                }

                Thread.Sleep(100);
            }
        }


        private void Audio_PlayPositionChanged(double ms)
        {
            //this.playPositionBlock.Content = string.Format("{0:0.0}", ms/1000d) + " s";
        }

        private void Audio_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            //this.playPositionBlock.Content = "Stopped";
        }

        private void Song_PatternChanged(Pattern pattern)
        {
            if (pattern != this.pattern)
            {
                return;
            }

            Env_SelectedPatternChanged(pattern);
        }

        private void Env_SelectedPatternChanged(Pattern pattern)
        {
            this.pattern = pattern;
            Clear();

            this.bpsTextBox.IsEnabled = pattern != null;

            if (pattern == null)
            {
                this.bpsTextBox.Text = string.Empty;
                this.nameTextBlock.Text = string.Empty;
                return;
            }

            this.bpsTextBox.Text = pattern.BPS.ToString();
            this.nameTextBlock.Text = this.pattern.Name;

            int i = 1;
            foreach (var track in this.pattern.Tracks)
            {
                var trackControl = new TrackControl($"Track {i}", track, this.pattern.Length);
                DockPanel.SetDock(trackControl, Dock.Left);
                this.tracksStackPanel.Children.Add(trackControl);
                i++;
            }
        }

        public void Clear()
        {
            this.tracksStackPanel.Children.Clear();
        }

        private void prevPatternButton_Click(object sender, RoutedEventArgs e)
        {
            if (Env.Song != null)
            {
                var idx = Env.SelectedPatternIdx - 1;
                if (idx < 0)
                {
                    idx = Env.Song.Patterns.Count - 1;
                }
                Env.OnSelectedPatternChanged(Env.Song.Patterns.Count == 0 ? -1 : idx);
            }
        }

        private void nextPatternButton_Click(object sender, RoutedEventArgs e)
        {
            if (Env.Song != null)
            {
                var idx = Env.SelectedPatternIdx + 1;
                if (idx >= Env.Song.Patterns.Count)
                {
                    idx = 0;
                }
                Env.OnSelectedPatternChanged(Env.Song.Patterns.Count == 0 ? -1 : idx);
            }
        }

        private void bpsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void bpsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prohibit space
            if (e.Key == Key.Enter)
            {
                if (double.TryParse(this.bpsTextBox.Text, out var bps))
                {
                    this.pattern.BPS = bps;
                    return;
                }
                this.bpsTextBox.Text = this.pattern.BPS.ToString();
            }
        }

        private void editPatternMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = EditPatternDialog.Create(this.pattern);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPatternChanged(this.pattern);
            }
        }
    }
}
