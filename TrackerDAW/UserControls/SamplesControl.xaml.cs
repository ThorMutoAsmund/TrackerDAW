using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for SamplesControl.xaml
    /// </summary>
    public partial class SamplesControl : UserControl
    {
        //private Point startPoint;

        public SamplesControl()
        {
            InitializeComponent();

            Song.SongChanged += Song_SongChanged;
            Env.Watchers.SamplesListChanged += Watchers_SamplesListChanged;
        }

        private void Song_SongChanged(Song song)
        {
            if (song == null)
            {
                this.DataContext = null;
            }
        }

        private void Watchers_SamplesListChanged(List<string> stringList)
        {
            this.DataContext = stringList;
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
                //AudioPlaybackEngine.Instance.StopPlayback();

                //var sampleName = this.samplesListView.SelectedItem as string;
                //AudioPlaybackEngine.Instance.PlaySound(Path.Combine(Env.Song.SamplesPath, sampleName));
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

            //    this.samplesListView.CheckDragDropMove<string>(diff, e.OriginalSource, "sample");
            //}
        }
    }
}
