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
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for EditNoteDialog.xaml
    /// </summary>
    public partial class EditNoteDialog : Window
    {
        private Note note;

        private EditNoteDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            this.partNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var partName = this.partNameTextBox.Text;
            var textContent = this.noteContentTextBox.Text;

            if (!Env.TryParseDouble(this.noteOffsetTextBox.Text, out var partOffset) || partOffset < 0d)
            {
                MessageBox.Show("Illegal part offset");
                return;
            }

            if (!Env.TryParseDouble(this.noteLengthTextBox.Text, out var noteLength) || noteLength < 0d)
            {
                MessageBox.Show("Illegal note length");
                return;
            }

            this.note.Name = partName;
            this.note.Offset = partOffset;
            this.note.Content = textContent;
            this.note.Length = noteLength;
            this.DialogResult = true;
        }

        public static void ShowDialog(Note note)
        {
            var dialog = new EditNoteDialog()
            {
                Owner = Env.MainWindow,
                note = note
            };

            dialog.partNameLabel.Content = note.Name;
            dialog.partNameTextBox.Text = note.Name;
            dialog.noteOffsetTextBox.Text = Env.TimeToString(note.Offset);
            dialog.noteContentTextBox.Text = note.Content;
            dialog.noteLengthTextBox.Text = Env.TimeToString(note.Length);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPartChanged(note);
            }
        }

        private void partNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.partNameLabel.Content = this.partNameTextBox.Text;
        }
    }
}
