using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;

namespace _2poc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IInputClient inputClient;
        private AsyncCallback inputHandlerCallback;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(this.inputClient != null)
            {
                this.inputClient.Close();
            }
        }

        private void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            int portNumber = Int32.Parse(Host_Port_Number_Textbox.Text);

            this.inputClient = new InputReceiver(portNumber);
            this.inputHandlerCallback = new AsyncCallback(handleInput);

            inputClient.BeginReceiveInput(this.inputHandlerCallback);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            string hostIpAndPortNumber = Client_Port_And_Ip_Textbox.Text;
            string[] splitConnectionDetails = hostIpAndPortNumber.Split(':');
            IPAddress ipAddress = IPAddress.Parse(splitConnectionDetails[0]);
            int portNumber = Int32.Parse(splitConnectionDetails[1]);

            this.inputClient = new InputSender(ipAddress, portNumber);
            this.inputClient.SendInputAsync(Client_Input_Textbox.Text);
        }

        public void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            byte[] input = inputClient.EndReceiveInput(inputResult, endpoint);

            inputClient.BeginReceiveInput(this.inputHandlerCallback);

            this.Dispatcher.Invoke(() =>
            {
                Host_Input_Textbox.Text += '\n' + Encoding.ASCII.GetString(input);
            });
        }
    }
}
