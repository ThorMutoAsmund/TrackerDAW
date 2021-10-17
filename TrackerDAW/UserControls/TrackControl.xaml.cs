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
                partControl.MouseDoubleClick += PartControl_MouseDoubleClick;
            }
        }

        private void PartControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var part = (sender as PartControl).Part;
            var dialog = EditPartDialog.Create(part);

            if (dialog.ShowDialog() == true)
            {
                Song.OnPartChanged(part);
            }

        }

        private void partCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetDataPresent("sample") || e.Data.GetDataPresent("script") || e.Data.GetDataPresent("part")))
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            var point = e.GetPosition(this.partCanvas);

            if (this.aimControl == null)
            {
                this.aimControl = new AimControl();
                this.partCanvas.Children.Add(this.aimControl);
            }
            this.aimControl.SetTop(GetSnapValue(point.Y));

            e.Effects = e.CheckEffect();
            e.Handled = true;
        }

        private void partCanvas_DragOver(object sender, DragEventArgs e)
        {
            partCanvas_DragEnter(sender, e);
        }

        private void partCanvas_Drop(object sender, DragEventArgs e)
        {
            var point = e.GetPosition(this.partCanvas);

            if (e.Data.GetDataPresent("sample"))
            {
                var sampleName = e.Data.GetData("sample") as string;

                this.track.AddPart(
                    new Part(GetSnapValue(point.Y) / Env.TrackPixelsPerSecond,
                    ProviderFactory.DefaultSampleProviderInfo,
                    new ProviderData()
                    {
                        { ProviderData.SampleNameKey, sampleName }
                    }));
            }
            else if (e.Data.GetDataPresent("part"))
            {
                (var part, var oldTrack) = ((Part, Track))e.Data.GetData("part");

                switch (e.CheckEffect())
                {
                    case DragDropEffects.Copy:
                        this.track.CopyPart(part, GetSnapValue(point.Y) / Env.TrackPixelsPerSecond);
                        break;
                    case DragDropEffects.Move:
                        this.track.MovePart(part, oldTrack, GetSnapValue(point.Y) / Env.TrackPixelsPerSecond);
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

        private void addEmptyPartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.track.AddPart(new Part(GetSnapValue(this.contextMenuPoint.Y) / Env.TrackPixelsPerSecond,
                ProviderFactory.EmptyProviderInfo, new ProviderData()));
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            this.contextMenuPoint = Mouse.GetPosition(this.partCanvas);
        }

        private double GetSnapValue(double y)
        {
            return Math.Floor(y / Env.DefaultPartHeight) * Env.DefaultPartHeight;
        }
    }
}
