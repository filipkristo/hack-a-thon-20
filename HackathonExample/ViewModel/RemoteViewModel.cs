using System.Threading.Tasks;
using System.Windows.Input;
using HackathonExample.Service;
using HackathonExample.Service.Interfaces;
using Xamarin.Essentials;

namespace HackathonExample.ViewModel
{
    public class RemoteViewModel : BaseViewModel
    {
        private ICommunicationService _communicationService;

        public string IpAddress
        {
            get => Get<string>();
            set => Set(value, afterChanged: () => AfterIpAddressChange(value));
        }

        public int? Port
        {
            get => Get<int?>();
            set => Set(value, afterChanged: () => AfterPortChange(value));
        }

        public ICommand MoveUpCommand => DefaultCommand(_communicationService.MoveDownAsync);

        public ICommand MoveDownCommand => DefaultCommand(_communicationService.MoveUpAsync);

        public ICommand MoveLeftCommand => DefaultCommand(_communicationService.MoveLeftAsync);

        public ICommand MoveRightCommand => DefaultCommand(_communicationService.MoveRightAsync);

        public ICommand StopCommand => DefaultCommand(_communicationService.StopAsync);

        public RemoteViewModel()
        {
            _communicationService = new UdpCommunicationService();

            if (Preferences.ContainsKey(nameof(IpAddress)))
                IpAddress = Preferences.Get(nameof(IpAddress), string.Empty);

            if (Preferences.ContainsKey(nameof(Port)))
                Port = Preferences.Get(nameof(Port), 0);
        }

        private void AfterIpAddressChange(string ipAddress)
        {
            Preferences.Set(nameof(IpAddress), ipAddress);

            _communicationService.Configure(ipAddress, Port ?? 0);
        }

        private void AfterPortChange(int? port)
        {
            if (port is { })
            {
                Preferences.Set(nameof(Port), port.Value);                
            }
            else
            {
                Preferences.Remove(nameof(Port));
            }

            _communicationService.Configure(IpAddress, port ?? 0);
        }
    }
}