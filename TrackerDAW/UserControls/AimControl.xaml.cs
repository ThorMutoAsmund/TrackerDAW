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
    /// Interaction logic for AimControl.xaml
    /// </summary>
    public partial class AimControl : UserControl
    {
        public AimControl()
        {
            InitializeComponent();

            this.Height = Env.DefaultPartHeight;
        }

        public void SetTop(double top)
        {
            Canvas.SetTop(this, top);
        }
        public void SetLeft(double left)
        {
            Canvas.SetLeft(this, left);
        }
    }
}
