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
    /// Interaction logic for TrackControl.xaml
    /// </summary>
    public partial class TrackControl : UserControl
    {
        private Track track;
        private Point contextMenuPoint;
        private AimControl aimControl;

        public TrackControl(string title, Track track, double paternLength)
        {
            InitializeComponent();

            this.track = track;
            this.Width = paternLength * Env.TrackPixelsPerSecond + grid.ColumnDefinitions[0].Width.Value;
            this.titleTextBlock.Text = title;

            Song.TrackChanged += Song_TrackChanged;

            RedrawTrack();
        }

        private void Song_TrackChanged(Track track)
        {
            if (track != this.track)
            {
                return;
            }

            RedrawTrack();
        }

        private void RedrawTrack()
        {
            this.partCanvas.Children.Clear();

            foreach (var part in this.track.Parts)
            {
                var partControl = new PartControl(part, this.track);
                this.partCanvas.Children.Add(partControl);
            }
        }

        private bool CheckDataPresent(DragEventArgs e)
        {
            if (!(e.Data.GetDataPresent("sample") || e.Data.GetDataPresent("part")))
            {
                e.Effects = DragDropEffects.None;
                return false;
            }
            return true;
        }

        private void EnsureAimControlCreated()
        {
            // Create or reuse aim control
            if (this.aimControl == null)
            {
                this.aimControl = new AimControl();
                this.partCanvas.Children.Add(this.aimControl);
            }
        }

        private void partCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!CheckDataPresent(e))
            {
                return;
            }

            EnsureAimControlCreated();

            // Aim control width
            if (e.Data.GetDataPresent("sample"))
            {
                this.aimControl.Width = 200d;

                // Aim control position
                var point = e.GetPosition(this.partCanvas);
                this.aimControl.SetLeft(point.X);
            }
            else if (e.Data.GetDataPresent("part"))
            {
                (var part, var oldTrack, var offset) = ((Part, Track, double))e.Data.GetData("part");

                var width = part.GetLength() * Env.TrackPixelsPerSecond;
                this.aimControl.Width = width;

                // Aim control position
                var point = e.GetPosition(this.partCanvas);
                this.aimControl.SetLeft(point.X - offset);
            }


            e.Effects = e.CheckEffect();
            e.Handled = true;
        }

        private void partCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (!CheckDataPresent(e))
            {
                return;
            }

            EnsureAimControlCreated();

            // Aim control width
            if (e.Data.GetDataPresent("sample"))
            {
                // Aim control position
                var point = e.GetPosition(this.partCanvas);
                this.aimControl.SetLeft(point.X);
            }
            else if (e.Data.GetDataPresent("part"))
            {
                var point = e.GetPosition(this.partCanvas);
                (var part, var oldTrack, var offset) = ((Part, Track, double))e.Data.GetData("part");
                var left = Math.Max(0d, point.X - offset);

                // Aim control position
                this.aimControl.SetLeft(left);
            }

            e.Effects = e.CheckEffect();
            e.Handled = true;
        }

        private void partCanvas_Drop(object sender, DragEventArgs e)
        {
            var point = e.GetPosition(this.partCanvas);
            
            if (e.Data.GetDataPresent("sample"))
            {
                var sampleName = e.Data.GetData("sample") as string;
                var left = Math.Max(0d, point.X);

                this.track.AddPart(
                    new Sample(left / Env.TrackPixelsPerSecond,
                    ProviderInfo.DefaultSampleProviderInfo,
                    new ProviderData()
                    {
                        { ProviderData.SampleNameKey, sampleName }
                    },
                    name: sampleName));
            }
            else if (e.Data.GetDataPresent("part"))
            {
                (var part, var oldTrack, var offset) = ((Part, Track, double))e.Data.GetData("part");
                var left = Math.Max(0d, point.X - offset);

                switch (e.CheckEffect())
                {
                    case DragDropEffects.Copy:
                        this.track.CopyPart(part, left / Env.TrackPixelsPerSecond);
                        break;
                    case DragDropEffects.Move:
                        this.track.MovePart(part, oldTrack, left / Env.TrackPixelsPerSecond);
                        break;
                }
            }

            partCanvas_DragLeave(sender, e);
        }

        private void partCanvas_DragLeave(object sender, DragEventArgs e)
        {
            if (this.aimControl != null)
            {
                this.partCanvas.Children.Remove(this.aimControl);
                this.aimControl = null;
            }
        }

        private void addEmptyCompositionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.track.AddPart(new Composition(this.contextMenuPoint.X / Env.TrackPixelsPerSecond,
                ProviderInfo.EmptyProviderInfo, new ProviderData(), name: "empty"));
        }

        private void addNoteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = StringAndTextDialog.Create("Create Note");
            if (dialog.ShowDialog() == true)
            {
                this.track.AddPart(new Note(this.contextMenuPoint.X / Env.TrackPixelsPerSecond, dialog.Value, dialog.TextContent));
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            this.contextMenuPoint = Mouse.GetPosition(this.partCanvas);
        }

        private void editTrackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditTrackDialog.ShowDialog(this.track, this.titleTextBlock.Text);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                e.Handled = true;
                EditTrackDialog.ShowDialog(this.track, this.titleTextBlock.Text);
            }
        }
    }
}
