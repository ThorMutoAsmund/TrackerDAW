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
                var registration = ProviderFactory.GetProviderRegistration(this.providerInfo);
                this.providerListView.SelectedValue = registration;
                this.providerTextBox.Text = registration.Name;
            }
        }

        private ProviderInfo providerInfo;

        public SelectProviderDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            var providers = ProviderFactory.ProviderRegistrations.Values;

            this.DataContext = providers;


            this.providerListView.SelectionChanged += ProviderListView_SelectionChanged;
            //this.patternNameTextBox.Focus();
        }

        private void ProviderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ProviderInfo = (this.providerListView.SelectedItem as ProviderRegistration).ProviderInfo;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static SelectProviderDialog Create(ProviderInfo providerInfo)
        {
            var dialog = new SelectProviderDialog()
            {
                Owner = Env.MainWindow
            };

            dialog.ProviderInfo = providerInfo;

            return dialog;
        }

        private void createBlankProviderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = StringDialog.Create("New provider name");
            if (dialog.ShowDialog() == true)
            {
                ProviderFactory.CreateBlankProviderScript(dialog.Value);
            }
        }

        private void duplicateProviderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
