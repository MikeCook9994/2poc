using System.Threading.Tasks;

namespace _2poc
{
    interface IInputClient
    {
        void SendInputAsync(string input);

        void ReceiveInputAsync(InputHandler inputHandler);

        void Close();
    }
}
