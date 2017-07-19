using System;
using System.Net;
using System.Windows;

using Microsoft.Practices.Unity;
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
        /// True if this application is functioning as the host; false if otherwise.
        /// </summary>
        bool isHost = false;

        /// <summary>
        /// Used to set hooks for monitoring keyboard inputs on the client's machine.
        /// </summary>
        KeyboardMonitor keyboardMonitor;

        /// <summary>
        /// Used to set hooks for monitoring mouse inputs on the client's machine.
        /// </summary>
        MouseMonitor mouseMonitor;

        /// <summary>
        /// Provides an interface for receiving <see cref="Input"/> input events over the network.
        /// </summary>
        IInputHost inputHost;

        /// <summary>
        /// Provides an interface for sending <see cref="Input"/> input events over the network.
        /// </summary>
        IInputClient inputClient;

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
            if(this.isHost == false)
            {
                this.keyboardMonitor.Dispose();
                this.mouseMonitor.Dispose();
                this.inputClient.Close();
            }
            else
            {
                this.inputHost.Close();
            }
        }

        /// <summary>
        /// Opens the host machine up to connections and prepares the <see cref="IInputSimulator"/> for simulating
        /// <see cref="Input"/> events when they are recieved from the client.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            this.isHost = true;

            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            // grabs the port number to accept connections on. Port Forwarding must be set up on your local gateway
            // to forward data over the port to the machine hosting the application.
            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);

            this.inputHost = new InputHost(portNumber);
       
            IInputSimulator inputSimulator = new InputSimulator();

            IKeyboard virtualKeyboard = new VirtualKeyboard(this.inputHost, inputSimulator, this);
            IMouse virtualMouse = new VirtualMouse(this.inputHost, inputSimulator, this);
        }

        /// <summary>
        /// Attempts to establish a connection with the host specified by the IP address and the Port.
        /// Additionally sets up keyboard and mouse hooks for sending input events to the host.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            this.isHost = false;

            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            //Parses the input string for the IP address and the port number.
            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            int portNumber = Int32.Parse(splitConnectionDetails[1]);

            this.inputClient = new InputClient();
            this.inputClient.Connect(ipAddress, portNumber);

            this.keyboardMonitor = new KeyboardMonitor(true);
            IKeyboard networkKeyboard = new NetworkKeyboard(this.inputClient, this.keyboardMonitor);

            this.mouseMonitor = new MouseMonitor(true);
            IMouse networkMouse = new NetworkMouse(this.inputClient, this.mouseMonitor);

            System.Windows.Forms.Application.Run();
        }
    }   
}
