using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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

            Env.MainWindow = this;
            Env.DirtyChanged += Env_DirtyChanged;
            Song.SongChanged += Song_SongChanged;

            Song_SongChanged(null);

            bool isActivated = false;
            this.Activated += (sender, e) =>
            {
                if (!isActivated)
                {
                    Song.CreateOrOpenDefault();
                    isActivated = true;

                    if (Env.Song.Patterns.Count > 0)
                    {
                        Env.OnSelectedPatternChanged(0);
                    }
                }
            };
        }

        private void Env_DirtyChanged(bool dirty)
        {
            this.Title = $"{this.title}{(dirty ? " *" : string.Empty)}";
        }

        private void Song_SongChanged(Song song)
        {
            var songNotNull = song != null;

            this.title = $"{Env.AppName}{(songNotNull ? $" - {song.Name}" : string.Empty)}" ;
            this.openMenu.IsEnabled = true;
            this.saveMenu.IsEnabled = songNotNull;
            this.closeMenu.IsEnabled = songNotNull;
            this.createPatternMenu.IsEnabled = songNotNull;
                        
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
                Env.OnApplicationEnded();
            }
        }

        private void Open_Action(object sender, RoutedEventArgs e)
        {
            if (!Dialogs.ConfirmChangesMade())
            {
                return;
            }

            if (Dialogs.OpenFile("Select project file", Env.ApplicationPath, out var projectFilePath, filter: "Project files (Project.json)|Project.json"))
            {
                var projectPath = System.IO.Path.GetDirectoryName(projectFilePath);

                if (!System.IO.Directory.Exists(projectPath))
                {
                    MessageBox.Show("Selected project not found");
                    return;
                }

                Song.Open(projectPath);
            }
        }

        private void Save_Action(object sender, RoutedEventArgs e)
        {
            if (!Env.HasChanges)
            {
                return;
            }

            Song.Save();
        }

        private void New_Action(object sender, RoutedEventArgs e)
        {
            if (!Dialogs.ConfirmChangesMade())
            {
                return;
            }

            var dialog = CreateProjectDialog.Create(Env.ApplicationPath, Env.NewProjectName, Env.DefaultSampleRate, Env.DefaultBPS, 2);
            if (dialog.ShowDialog() ?? false)
            {
                if (!System.IO.Directory.Exists(dialog.ProjectPath))
                {
                    MessageBox.Show("Selected project folder not found");
                    return;
                }

                var projectPath = System.IO.Path.Combine(dialog.ProjectPath, dialog.ProjectName);

                if (System.IO.Directory.Exists(projectPath))
                {
                    MessageBox.Show("Project already exists");
                    return;
                }

                Song.CreateNew(projectPath, dialog.ProjectName, dialog.SampleRate, dialog.BPS);
            }
        }

        private void Close_Action(object sender, RoutedEventArgs e)
        {
            Song.Close();
        }

        private void Exit_Action(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void createPatternMenu_Click(object sender, RoutedEventArgs e)
        {
            Env.Song.NewPattern(Env.DefaultPatternLength, Env.Song.BPS);
        }

        private void Play_Action(object sender, RoutedEventArgs e)
        {
            Audio.Play();
        }

        private void PlayPattern_Action(object sender, RoutedEventArgs e)
        {
            Audio.PlayFromPatternStart(Env.SelectedPattern);
        }

        private void PlayFromStart_Action(object sender, RoutedEventArgs e)
        {
            Audio.PlayFromStart();
        }

        private void Stop_Action(object sender, RoutedEventArgs e)
        {
            Audio.Stop();
        }
    }
}
