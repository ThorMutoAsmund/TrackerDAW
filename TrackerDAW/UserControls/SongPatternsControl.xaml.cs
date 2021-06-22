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
    /// Interaction logic for SongPatternsControl.xaml
    /// </summary>
    public partial class SongPatternsControl : UserControl
    {
        public SongPatternsControl()
        {
            InitializeComponent();

            Song.SongChanged += Song_SongChanged;
            Song.PatternsChanged += Song_PatternsChanged;
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

        private void Song_PatternsChanged()
        {
            this.DataContext = Env.Song.Patterns.ToArray();
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Env.OnSelectedPatternChanged(this.listView.SelectedIndex);
        }

        private void duplicateMenu_Click(object sender, RoutedEventArgs e)
        {
            var pattern = this.listView.SelectedItem as Pattern;
            Env.Song.AddPattern(pattern, this.listView.SelectedIndex);
        }
    }
}