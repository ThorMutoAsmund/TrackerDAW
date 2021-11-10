﻿using System;
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
    /// Interaction logic for SongPartsControl.xaml
    /// </summary>
    public partial class SongPartsControl : UserControl
    {
        public SongPartsControl()
        {
            InitializeComponent();

            Song.SongChanged += Song_SongChanged;
            Song.TrackChanged += Song_TrackChanged;
        }

        private void Song_SongChanged(Song song, SongChangedAction action)
        {
            if (action == SongChangedAction.Closed)
            {
                this.DataContext = null;
                return;
            }

            Song_TrackChanged(null);
        }

        private void Song_TrackChanged(Track obj)
        {
            this.DataContext = Env.Song.Patterns.SelectMany(p => p.Tracks.SelectMany(t => t.Parts)).Distinct().ToArray();
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void editMenu_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView.SelectedItem != null)
            {
                var part = this.listView.SelectedItem as Part;
                PartControl.EditPart(part);
            }
        }
    }
}
