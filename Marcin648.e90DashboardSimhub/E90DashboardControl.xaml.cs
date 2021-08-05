using System.Windows.Controls;
using System.Diagnostics;
using System.IO.Ports;

namespace E90DashboardSimhub {
    /// <summary>
    /// Logika interakcji dla klasy E90DashboardControl.xaml
    /// </summary>
    public partial class E90DashboardControl : UserControl {

        public E90Dashboard Plugin { get; }
        public E90DashboardControl() {
            InitializeComponent();
        }

        public E90DashboardControl(E90Dashboard plugin) : this() {
            Plugin = plugin;
            RefrashPorts();
        }

        private void RefrashPorts() {
            string[] ports = SerialPort.GetPortNames();
            portComboBox.ItemsSource = ports;
        }

        private void AuthorLink_Click(object sender, System.Windows.RoutedEventArgs e) {
            Process.Start("explorer", "https://github.com/Marcin648");
        }

        private void ConnectButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            string portName = portComboBox.Text;
            try {
                Plugin.Connect(portName);
                statusLabel.Content = "Connected " + portName;
            } catch (System.ArgumentException){
                Plugin.Disconnect();
            } catch (System.UnauthorizedAccessException) {
                Plugin.Disconnect();
            }
        }

        private void DisconnectButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            Plugin.Disconnect();
            statusLabel.Content = "Disconnected";
        }
    }
}
