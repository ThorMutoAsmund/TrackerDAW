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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for SongPatternsControl.xaml
    /// </summary>
    public partial class SongPatternsControl : UserControl
    {
        public SongPatternsControl()
        {
            InitializeComponent();

            Env.SelectedPatternChanged += Env_SelectedPatternChanged;
            Song.SongChanged += Song_SongChanged;
            Song.PatternsChanged += Song_PatternsChanged;
            Song.PatternChanged += Song_PatternChanged;
        }

        private void Song_SongChanged(Song song)
        {
            if (song == null)
            {
                this.DataContext = null;
                return;
            }

            Song_PatternsChanged();
        }

        private void Env_SelectedPatternChanged(Pattern pattern)
        {
            if (this.listView.SelectedItem != pattern)
            {
                this.listView.SelectedItem = pattern;
            }
        }

        private void Song_PatternsChanged()
        {
            this.DataContext = Env.Song.Patterns.ToArray();
        }

        private void Song_PatternChanged(Pattern pattern)
        {
            this.DataContext = Env.Song.Patterns.ToArray();
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && this.listView.SelectedIndex != -1)
            {
                Env.OnSelectedPatternChanged(this.listView.SelectedIndex);
            }
        }

        private void editMenu_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView.SelectedItem != null)
            {
                var pattern = this.listView.SelectedItem as Pattern;
                EditPatternDialog.ShowDialog(pattern);
            }
        }

        private void duplicateMenu_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView.SelectedItem != null)
            {
                var pattern = this.listView.SelectedItem as Pattern;
                var dialog = StringDialog.Create("New pattern name", pattern.Name);
                if (dialog.ShowDialog() == true)
                {
                    Env.Song.AddPattern(pattern.Clone(dialog.Value), this.listView.SelectedIndex + 1);
                }
            }
        }
    }
}
