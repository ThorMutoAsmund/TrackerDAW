using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for PartControl.xaml
    /// </summary>
    public partial class PartControl : UserControl
    {
        public Part Part => this.part;

        private Part part;
        private Track track;
        private Point dragStartPoint;

        public PartControl(Part part, Track track)
        {
            InitializeComponent();

            this.part = part;
            this.track = track;
            
            Song.PartChanged += Song_PartChanged;

            RedrawPart();
        }

        private void Song_PartChanged(Part part)
        {
            if (part != this.part)
            {
                return;
            }

            RedrawPart();
        }

        private void RedrawPart()
        {
            this.titleTextBlock.Text = this.part.Name;
            this.Width = this.part.GetLength() * Env.TrackPixelsPerSecond;
            Canvas.SetLeft(this, this.part.Offset * Env.TrackPixelsPerSecond);
        }

        private void grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dragStartPoint = e.GetPosition(null);
        }

        private void grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Get the current mouse position
                var mousePos = e.GetPosition(null);
                var diff = this.dragStartPoint - mousePos;
                var canvasLeft = Canvas.GetLeft(this);
                var offset = e.GetPosition(this).X;
                this.CheckDragDrop(diff, (this.part, this.track, offset), "part", DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void editPartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditPartDialog.ShowDialog(this.part);
        }

        private void deletePartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.track.DeletePart(this.part);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPartDialog.ShowDialog(this.part);

        }
    }
}
