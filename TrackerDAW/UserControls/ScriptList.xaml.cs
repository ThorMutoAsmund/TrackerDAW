using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for ScriptList.xaml
    /// </summary>
    public partial class ScriptList : UserControl
    {
        //private Point startPoint;

        public ScriptList()
        {
            InitializeComponent();

            Song.SongChanged += Song_SongChanged;
            Env.Watchers.ScriptsListChanged += Watchers_ScriptsListChanged; 
        }
        
        private void Song_SongChanged(Song song, SongChangedAction action)
        {
            if (action == SongChangedAction.Closed)
            {
                this.DataContext = null;
            }
        }

        private void Watchers_ScriptsListChanged(List<string> stringList)
        {
            this.DataContext = stringList;
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
            //    var fileName = this.scriptsListView.SelectedItem as string;
            //    Env.Song.EditScript(fileName);
            //}

            //this.startPoint = e.GetPosition(null);
        }

        private void listView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    // Get the current mouse position
            //    var mousePos = e.GetPosition(null);
            //    var diff = startPoint - mousePos;

            //    this.scriptsListView.CheckDragDropMove<string>(diff, e.OriginalSource, "script");
            //}
        }
    }
}
