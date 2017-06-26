using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;

namespace _2poc
{
    public delegate void InputHandler(Task<UdpReceiveResult> inputTask);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IInputClient inputClient;
        InputHandler inputHandler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.inputClient.Close();
        }

        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);

            this.inputClient = new InputReceiver(portNumber);
            this.inputHandler = new InputHandler(handleInput);

            inputClient.ReceiveInputAsync(inputHandler);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            int portNumber = Int32.Parse(splitConnectionDetails[1]);

            this.inputClient = new InputSender(ipAddress, portNumber);
            this.inputClient.SendInputAsync("Hello World");
        }

        public void handleInput(Task<UdpReceiveResult> inputTask)
        {
            inputTask.Wait();
            this.inputClient.ReceiveInputAsync(this.inputHandler);
            byte[] input = inputTask.Result.Buffer;
            Host_Input_Textbox.Text += '\n' + Encoding.ASCII.GetString(input);
        }
    }
}
