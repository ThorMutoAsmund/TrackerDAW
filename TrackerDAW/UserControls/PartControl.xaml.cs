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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for PartControl.xaml
    /// </summary>
    public partial class PartControl : UserControl
    {
        private Part part;
        Track track;
        private Point dragStartPoint;

        public PartControl(Part part, Track track)
        {
            InitializeComponent();

            this.part = part;
            this.track = track;
            this.Height = Env.DefaultPartHeight;
            Canvas.SetTop(this, this.part.Start * Env.TrackPixelsPerSecond);
            
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
            this.titleTextBlock.Text = this.part.Title;
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
                
                this.CheckDragDrop(diff, (this.part, this.track), "part", DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void deletePartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.track.DeletePart(this.part);
        }
    }
}
