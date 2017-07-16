using System;
using System.Net;
using System.Windows;

using WindowsInput;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes the Main Window and it's components.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the network connection and unsets the hooks when closing the GUI.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        /// <summary>
        /// Opens the host machine up to connections and prepares the <see cref="IInputSimulator"/> for simulating
        /// <see cref="Input"/> events when they are recieved from the client.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            // grabs the port number to accept connections on. Port Forwarding must be set up on your local gateway
            // to forward data over the port to the machine hosting the application.
            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);

            IInputHost inputReceiver = new InputHost(portNumber);
       
            IInputSimulator inputSimulator = new InputSimulator();

            IKeyboard virtualKeyboard = new VirtualKeyboard(inputReceiver, inputSimulator, this);
            IMouse virtualMouse = new VirtualMouse(inputReceiver, inputSimulator, this);
        }

        /// <summary>
        /// Attempts to establish a connection with the host specified by the IP address and the Port.
        /// Additionally sets up keyboard and mouse hooks for sending input events to the host.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            //Parses the input string for the IP address and the port number.
            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            int portNumber = Int32.Parse(splitConnectionDetails[1]);

            IInputClient inputSender = new InputClient();
            inputSender.Connect(ipAddress, portNumber);

            KeyboardMonitor keyboardMonitor = new KeyboardMonitor(true);
            IKeyboard networkKeyboard = new NetworkKeyboard(inputSender, keyboardMonitor);

            MouseMonitor mouseMonitor = new MouseMonitor(true);
            IMouse networkMouse = new NetworkMouse(inputSender, mouseMonitor);

            System.Windows.Forms.Application.Run();
        }
    }   
}
