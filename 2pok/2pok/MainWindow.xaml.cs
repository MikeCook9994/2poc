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
        IMouse mouse;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);

            IInputReceiver inputReciever = new InputReceiver(portNumber);

            IInputSimulator inputSimulator = new InputSimulator();

            VirtualKeyboard virtualKeyboard = new VirtualKeyboard(inputReciever, inputSimulator, this);
            this.keyboard = virtualKeyboard;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            //string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            //string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            //IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            //int portNumber = Int32.Parse(splitConnectionDetails[1]);

            IInputSender inputSender = new InputSender();
            //inputSender.Connect(ipAddress, portNumber);

            KeyboardMonitor keyboardMonitor = new KeyboardMonitor(true);

            IKeyboard networkKeyboard = new NetworkKeyboard(inputSender, keyboardMonitor);
            this.keyboard = networkKeyboard;

            MouseMonitor mouseMonitor = new MouseMonitor(true);

            IMouse networkMouse = new NetworkMouse(inputSender, mouseMonitor);
            this.mouse = networkMouse;

            System.Windows.Forms.Application.Run();
        }
    }   
}
