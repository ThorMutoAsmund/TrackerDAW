using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string title;

        public MainWindow()
        {
            InitializeComponent();

            Env.DirtyChanged += Env_DirtyChanged;
            Song.SongChanged += Song_SongChanged;

            Song_SongChanged(null);

            bool isActivated = false;
            this.Activated += (sender, e) =>
            {
                if (!isActivated)
                {
                    Song.CreateEmpty();
                    isActivated = true;
                }
            };
        }

        private void Env_DirtyChanged(bool dirty)
        {
            this.Title = $"{this.title}{(dirty ? " *" : string.Empty)}";
        }

        private void Song_SongChanged(Song song)
        {
            var songExists = song != null;

            this.title = $"{Env.AppName}{(songExists ? $" - {song.Name}" : string.Empty)}" ;
            this.openMenu.IsEnabled = false;
            this.saveMenu.IsEnabled = false;
            this.closeMenu.IsEnabled = songExists;
            this.createPatternMenu.IsEnabled = songExists;
                        
            Env_DirtyChanged(Env.HasChanges);
        }

        //private void Song_AvailablePatternsChanged()
        //{
        //    this.patternsMenu.Items.Clear();
        //    this.patternsMenu.IsEnabled = Env.Song != null;
        //    if (Env.Song == null)
        //    {
        //        return;
        //    }

        //    foreach (var pattern in Env.Song.AvailablePatterns)
        //    {
        //        var newMenuItem = new MenuItem();
        //        newMenuItem.Header = pattern.Name;
        //        this.patternsMenu.Items.Add(newMenuItem);
        //        newMenuItem.Click += new RoutedEventHandler(patternMenu_Click);
        //    }
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !Dialogs.ConfirmChangesMade();
            if (!e.Cancel)
            {
                //AudioPlaybackEngine.Instance.Dispose();
            }
        }
        
        private void newMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not supported yet");

            //Env.OnSelectedPatternChanged(0);
        }

        private void closeMenu_Click(object sender, RoutedEventArgs e)
        {
            Song.Close();
        }

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void createPatternMenu_Click(object sender, RoutedEventArgs e)
        {
            Env.Song.NewPattern(Env.DefaultPatternLength, Env.Song.BPS);
        }

        private void patternMenu_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
