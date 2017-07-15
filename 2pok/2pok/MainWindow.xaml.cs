using System;
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

            IInputHost inputReceiver = new InputHost(portNumber);
       
            IInputSimulator inputSimulator = new InputSimulator();

            VirtualKeyboard virtualKeyboard = new VirtualKeyboard(inputReceiver, inputSimulator, this);
            this.keyboard = virtualKeyboard;

            VirtualMouse virtualMouse = new VirtualMouse(inputReceiver, inputSimulator, this);
            this.mouse = virtualMouse;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            OpenConnection.IsEnabled = false;
            Connect.IsEnabled = false;

            //string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            //string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            //IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            //int portNumber = Int32.Parse(splitConnectionDetails[1]);

            IInputClient inputSender = new InputClient();
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
