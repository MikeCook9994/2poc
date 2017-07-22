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
        /// -1 if the client has not been configured as host or client. 0 if client, 1 if host.
        /// </summary>
        int isHost = -1;

        /// <summary>
        /// Used to set hooks for monitoring keyboard inputs on the client's machine.
        /// </summary>
        KeyboardMonitor keyboardMonitor;

        /// <summary>
        /// Used to set hooks for monitoring mouse inputs on the client's machine.
        /// </summary>
        MouseMonitor mouseMonitor;

        /// <summary>
        /// Provides an interface for receiving <see cref="Input"/> keyboard input events over the network.
        /// </summary>
        IInputHost keyboardInputHost;

        /// <summary>
        /// Provides an interface for receiving <see cref="Input"/> mouse input events over the network.
        /// </summary>
        IInputHost mouseInputHost;

        /// <summary>
        /// Provides an interface for sending <see cref="Input"/> keyboard input events over the network.
        /// </summary>
        IInputClient keyboardInputClient;

        /// <summary>
        /// Provides an interface for sending <see cref="Input"/> mouse input events over the network.
        /// </summary>
        IInputClient mouseInputClient;

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
            if(this.isHost == 0)
            {
                this.keyboardMonitor.Dispose();
                this.keyboardInputClient.Close();

                this.mouseMonitor.Dispose();
                this.mouseInputClient.Close();
            }
            else if(this.isHost == 1)
            {
                this.mouseInputHost.Close();
                this.keyboardInputHost.Close();
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
            this.isHost = 1;

            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            // grabs the port number to accept connections on. Port Forwarding must be set up on your local gateway
            // to forward data over the port to the machine hosting the application.
            string portRange = Host_Port_Number_Textbox.Text;
            string[] portExtremes = portRange.Split(';');
            int mousePortNumber = Int32.Parse(portExtremes[0]);
            int keyboardPortNumber = Int32.Parse(portExtremes[1]);

            this.mouseInputHost = new InputHost(mousePortNumber);
            this.keyboardInputHost = new InputHost(keyboardPortNumber);

            IInputSimulator inputSimulator = new InputSimulator();

            IMouse virtualMouse = new VirtualMouse(this.mouseInputHost, inputSimulator, this);
            IKeyboard virtualKeyboard = new VirtualKeyboard(this.keyboardInputHost, inputSimulator, this);
        }

        /// <summary>
        /// Attempts to establish a connection with the host specified by the IP address and the Port.
        /// Additionally sets up keyboard and mouse hooks for sending input events to the host.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            this.isHost = 0;

            //Disables the connection buttons so a second connection cannot attempt to be established.
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            //Parses the input string for the IP address and the port number.
            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');

            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);

            string[] portExtremes = splitConnectionDetails[1].Split(';');
            int mousePortNumber = Int32.Parse(portExtremes[0]);
            int keyboardPortNumber = Int32.Parse(portExtremes[1]);

            this.mouseInputClient = new InputClient();
            this.mouseInputClient.Connect(ipAddress, mousePortNumber);

            this.keyboardInputClient = new InputClient();
            this.keyboardInputClient.Connect(ipAddress, keyboardPortNumber);

            this.keyboardMonitor = new KeyboardMonitor(true);
            IKeyboard networkKeyboard = new NetworkKeyboard(this.keyboardInputClient, this.keyboardMonitor);

            this.mouseMonitor = new MouseMonitor(true);
            IMouse networkMouse = new NetworkMouse(this.mouseInputClient, this.mouseMonitor);

            System.Windows.Forms.Application.Run();
        }
    }   
}
