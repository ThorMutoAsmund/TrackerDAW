using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TrackerDAW
{
    /// <summary>
    /// Interaction logic for TimeRuler.xaml
    /// </summary>
    public partial class TimeRuler : UserControl
    {
        public Pattern Pattern 
        {
            get => this.pattern;
            set
            {
                this.pattern = value;

                InvalidateVisual();
            }
        }

        private Pattern pattern;
        private Pen linePen = new Pen(Brushes.White, 1.0);
        private Brush textBrush = new SolidColorBrush(Color.FromRgb(150,150,150));

        public TimeRuler()
        {
            InitializeComponent();

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.pattern == null)
            {
                this.Width = 0;
                return;
            }

            this.Width = this.pattern.Length * Env.TrackPixelsPerSecond;

            var ct = this.Width / 50d;
            var pt = this.pattern.Length / ct;

            var  intervals = new (double,int)[] { (0.01d, 2), (0.02d, 2), (0.05d, 2), (0.1d, 1), (0.2d, 1), (0.5d, 1), (1d, 0), (2d, 0), (5d, 0), (10d, 0) };
            var ci = intervals[0].Item1;
            var cp = intervals[0].Item2;
            foreach (var (interval,precision) in intervals)
            {
                if (interval > pt)
                {
                    break;
                }
                ci = interval;
                cp = precision;
            }

            var brush = Application.Current.TryFindResource("TimeRulerBackground") as SolidColorBrush;
            drawingContext.DrawRectangle(brush, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            for (var i = 0d; i < this.pattern.Length; i += ci)
            {
                var j = i * Env.TrackPixelsPerSecond;

                drawingContext.DrawLine(linePen, new Point(j, this.ActualHeight-1),
                    new Point(j, 0));
                
                FormattedText formattedText = new FormattedText($"{Env.TimeToStringPrecision(i, cp)} s", 
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight, 
                    new Typeface("Segoe UI"), 12,
                    this.textBrush,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                drawingContext.DrawText(formattedText, new Point(j + 3, 0));
            }
        }
    }
}
