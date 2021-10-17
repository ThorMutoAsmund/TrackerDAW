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
    /// Interaction logic for CreateProjectDialog.xaml
    /// </summary>
    public partial class CreateProjectDialog : Window
    {
        public string ProjectPath => this.projectPathTextBox.Text;
        public string ProjectName => this.projectNameTextBox.Text;
        public int SampleRate => Int32.TryParse(this.sampleRateTextBox.Text, out var sampleRate) ? sampleRate : Env.DefaultSampleRate;
        public double BPS => Int32.TryParse(this.bpsTextBox.Text, out var sampleRate) ? sampleRate : Env.DefaultSampleRate;

        public CreateProjectDialog()
        {
            InitializeComponent();

            this.okButton.Click += (sender, e) => this.DialogResult = true;
        }

        public static CreateProjectDialog Create(string initialProjectPath, string projectName, int sampleRate, double bps, int channels)
        {
            var dialog = new CreateProjectDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.projectPathTextBox.Text = initialProjectPath;
            dialog.projectNameTextBox.Text = projectName;
            dialog.sampleRateTextBox.Text = $"{sampleRate}";
            dialog.bpsTextBox.Text = $"{bps}";

            return dialog;
        }
    }
}
