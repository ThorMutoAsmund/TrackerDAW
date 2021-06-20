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
            this.Height = paternLength * Env.TrackPixelsPerSecond;
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

        private void partCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetDataPresent("sample") || e.Data.GetDataPresent("script") || e.Data.GetDataPresent("part")))
            {
                return;
            }

            var point = e.GetPosition(this.partCanvas);

            if (this.aimControl == null)
            {
                this.aimControl = new AimControl();
                this.partCanvas.Children.Add(this.aimControl);
            }
            this.aimControl.SetTop(GetSnapValue(point.Y));

            e.Effects = DragDropEffects.All;
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

                this.track.AddPart(new SampleProvider(sampleName), GetSnapValue(point.Y) / Env.TrackPixelsPerSecond);
            }
            else if (e.Data.GetDataPresent("part"))
            {
                (var part, var oldTrack) = ((Part, Track))e.Data.GetData("part");

                this.track.MovePart(part, oldTrack, GetSnapValue(point.Y) / Env.TrackPixelsPerSecond);
            }

            if (this.aimControl != null)
            {
                this.partCanvas.Children.Remove(this.aimControl);
                this.aimControl = null;
            }

            //    if (e.Data.GetDataPresent("sample"))
            //    {
            //        var sampleName = e.Data.GetData("sample") as string;

            //        var sampleId = Env.Song.FindSample(sampleName).Id;
            //        var sampleLength = SampleDataProcessor.GetSampleLength(sampleId);

            //        if (sampleLength == 0)
            //        {
            //            MessageBox.Show("Empty or unsupported file");
            //            return;
            //        }

            //        var part = AddPart(point: point, title: sampleName);
            //        var generator = part.AddSampleGenerator(sampleName);

            //        // Set length
            //        if (generator.Settings.ContainsKey(Tags.SampleId))
            //        {
            //            part.SampleLength = sampleLength;
            //            Env.Song.OnPartChanged(part);
            //        }
            //    }
            //    else if (e.Data.GetDataPresent("script"))
            //    {
            //        var scriptName = e.Data.GetData("script") as string;

            //        var part = AddPart(point: point, title: scriptName);
            //        part.AddGenerator(scriptName);
            //    }
            //}

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
            this.track.AddEmptyPart(this.contextMenuPoint.Y / Env.TrackPixelsPerSecond);
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
