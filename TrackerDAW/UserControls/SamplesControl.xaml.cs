using NAudio.Wave;
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
        private Point dragStartPoint;

        public SamplesControl()
        {
            InitializeComponent();

            Song.SongChanged += Song_SongChanged;
            Env.Watchers.SamplesListChanged += Watchers_SamplesListChanged;
        }

        private void Song_SongChanged(Song song, SongChangedAction action)
        {
            if (action == SongChangedAction.Closed)
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
            if (this.listView.SelectedItem != null)
            {
                var fileName = this.listView.SelectedItem as string;
                Audio.PlayFile(fileName);
            }
        }

        private void listView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Audio.Stop();
            this.dragStartPoint = e.GetPosition(null);
        }

        private void listView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this.listView.SelectedItem != null)
            {
                // Get the current mouse position
                var mousePos = e.GetPosition(null);
                var diff = this.dragStartPoint - mousePos;

                var sampleName = this.listView.SelectedItem as string;
                this.listView.CheckDragDrop(diff, () => {
                    var length = DefaultSampleProvider.GetFileLength(Env.Song, sampleName);
                    return (sampleName, length);
                }
                , DragDropKey.Sample);
            }
        }
    }
}
