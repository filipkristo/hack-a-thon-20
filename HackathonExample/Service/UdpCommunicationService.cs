using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HackathonExample.Consts;
using HackathonExample.Service.Interfaces;

namespace HackathonExample.Service
{
    public class UdpCommunicationService : ICommunicationService
    {
        private string _ipAddress;
        private int _port;

        public UdpCommunicationService()
        {
        }

        public void Configure(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public Task MoveDownAsync() => SendUdpCommandAsync(Command.Down);

        public Task MoveLeftAsync() => SendUdpCommandAsync(Command.Left);

        public Task MoveRightAsync() => SendUdpCommandAsync(Command.Right);

        public Task MoveUpAsync() => SendUdpCommandAsync(Command.Up);

        private async Task SendUdpCommandAsync(string command)
        {
            using var client = new UdpClient(_ipAddress, _port);

            var bytes = Encoding.UTF8.GetBytes(command);
            await client.SendAsync(bytes, bytes.Length).ConfigureAwait(false);
        }
    }
}
