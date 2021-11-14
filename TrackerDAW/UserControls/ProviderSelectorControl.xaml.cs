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
    /// Interaction logic for ProviderSelectorControl.xaml
    /// </summary>
    public partial class ProviderSelectorControl : UserControl
    {
        public ProviderInfo ProviderInfo
        {
            get => this.providerInfo;
            set
            {
                this.providerInfo = value;
                var providerClass = ProviderFactory.Default.GetProviderClass(this.providerInfo);
                this.providerTextBox.Text = providerClass != null ? providerClass.Name : string.Empty;
            }
        }

        private ProviderInfo providerInfo;

        public ProviderSelectorControl()
        {
            InitializeComponent();
        }

        private void providerSelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDialog();
        }

        private void OpenDialog()
        { 
            var dialog = SelectProviderDialog.Create(this.providerInfo);
            if (dialog.ShowDialog() == true)
            {
                this.ProviderInfo = dialog.ProviderRegistration.ToProviderInfo();
            }
        }

        private void providerTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenDialog();
        }
    }
}
