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
    /// Interaction logic for SelectProviderDialog.xaml
    /// </summary>
    public partial class SelectProviderDialog : Window
    {
        public ProviderInfo ProviderInfo
        {
            get => this.providerInfo;
            set
            {
                this.providerInfo = value;
            }
        }

        private ProviderInfo providerInfo;

        public SelectProviderDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            //this.patternNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Assign seleced provider info

            this.DialogResult = true;
        }

        public static SelectProviderDialog Create(ProviderInfo providerInfo)
        {
            var dialog = new SelectProviderDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.ProviderInfo = providerInfo;

            //dialog.projectPathTextBox.Text = initialProjectPath;

            return dialog;
        }
    }
}
