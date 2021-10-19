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
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for EditPatternDialog.xaml
    /// </summary>
    public partial class EditPartDialog : Window
    {
        private Part part;

        public EditPartDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.partNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var partName = this.partNameTextBox.Text;

            if (!Env.TryParseDouble(this.partOffsetTextBox.Text, out var partOffset) || partOffset < 0d)
            {
                MessageBox.Show("Illegal part offset");
                return;
            }

            if (!Env.TryParseDouble(this.partGainTextBox.Text, out var partGain) || partGain < 0d)
            {
                MessageBox.Show("Illegal part gain");
                return;
            }

            this.part.Name = partName;
            this.part.Offset = partOffset;
            this.part.Gain = partGain;
            this.DialogResult = true;
        }

        public static void ShowDialog(Part part)
        {
            var dialog = new EditPartDialog()
            {
                Owner = Env.MainWindow,
                part = part
            };

            dialog.partNameLabel.Content = part.Name;
            dialog.partNameTextBox.Text = part.Name;
            dialog.partGainTextBox.Text = Env.GainToString(part.Gain);
            dialog.partOffsetTextBox.Text = Env.TimeToString(part.Offset);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPartChanged(part);
            }
        }

        private void partNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.partNameLabel.Content = this.partNameTextBox.Text;
        }
    }
}
