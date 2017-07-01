using System.Windows;
using System.Net;
using System;
using _2pok.interfaces;
using WindowsInput;

namespace _2pok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IKeyboard keyboard;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.keyboard.Disconnect();
        }

        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            OpenConnection.IsEnabled = false;

            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);
            IInputReceiver inputNetworkClient = new InputReceiver(portNumber);
            IInputSimulator inputSimulator = new InputSimulator();

            VirtualKeyboard virtualKeyboard = new VirtualKeyboard(inputNetworkClient, inputSimulator,  this);
            this.keyboard = virtualKeyboard;

            virtualKeyboard.Connect();


        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Connect.IsEnabled = false;

            IInputSender inputNetworkClient = new InputSender();
            KeyboardMonitor keyboardMonitor = new KeyboardMonitor(true);

            INetworkKeyboard networkKeyboard = new NetworkKeyboard(inputNetworkClient, keyboardMonitor);
            this.keyboard = networkKeyboard;

            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            int portNumber = Int32.Parse(splitConnectionDetails[1]);

            networkKeyboard.Connect(ipAddress, portNumber);
        }
    }   
}
