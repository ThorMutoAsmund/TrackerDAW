using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for ScriptsListControl.xaml
    /// </summary>
    public partial class ScriptsListControl : UserControl
    {
        private Point dragStartPoint;

        public ScriptsListControl()
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

            IDEIntegration.TestBuildProject(Env.Song.ScriptsPath);
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext is string scriptFileName)
            {
                IDEIntegration.OpenSourceFile(Env.Song.ScriptsPath, scriptFileName);
            }
        }

        private void listView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragStartPoint = e.GetPosition(null);
        }

        private void listView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed && this.listView.SelectedItem != null)
            //{
            //    // Get the current mouse position
            //    var mousePos = e.GetPosition(null);
            //    var diff = this.dragStartPoint - mousePos;

            //    var sampleName = this.listView.SelectedItem as string;
            //    this.listView.CheckDragDrop(diff, () => {
            //        var length = DefaultSampleProvider.GetFileLength(Env.Song, sampleName);
            //        return (sampleName, length);
            //    }
            //    , DragDropKey.Sample);
            //}
        }
    }
}
