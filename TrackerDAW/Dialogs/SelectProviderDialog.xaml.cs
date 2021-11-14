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
        private bool viewOnly;

        public ProviderRegistration ProviderRegistration
        {
            get => this.providerRegistration;
            set
            {
                this.providerRegistration = value;
                this.providerListView.SelectedValue = value;
                this.providerTextBox.Text = value.Name;
            }
        }

        private ProviderRegistration providerRegistration;

        private SelectProviderDialog()
        {
            InitializeComponent();

            this.okButton.Click += OkButton_Click;

            var providers = ProviderFactory.Default.ProviderRegistrations.Values;

            this.DataContext = providers;


            this.providerListView.SelectionChanged += ProviderListView_SelectionChanged;
            //this.patternNameTextBox.Focus();
        }

        private void ProviderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.viewOnly)
            {
                this.ProviderRegistration = this.providerListView.SelectedItem as ProviderRegistration;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static SelectProviderDialog Create(ProviderInfo providerInfo = null)
        {
            var dialog = new SelectProviderDialog()
            {
                Owner = Env.MainWindow,
                viewOnly = providerInfo == null
            };


            if (providerInfo != null)
            {
                dialog.ProviderRegistration = ProviderFactory.Default.GetProviderRegistration(providerInfo);
            }
            else
            {
                dialog.cancelButton.Visibility = Visibility.Hidden;
                dialog.dialogTitle.Content = "Provider List";
                dialog.Title = "Provider List";
            }

            return dialog;
        }

        private void createBlankProviderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = StringDialog.Create("New provider name");
            if (dialog.ShowDialog() == true)
            {
                ProviderFactory.Default.CreateBlankProviderScript(dialog.Value);
            }
        }

        private void duplicateProviderButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
