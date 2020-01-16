using System.Windows.Input;
using HackathonExample.Service;
using HackathonExample.Service.Interfaces;

namespace HackathonExample.ViewModel
{
    public class RemoteViewModel : BaseViewModel
    {
        private ICommunicationService _communicationService;

        public string IpAddress
        {
            get => Get<string>();
            set => Set(value, afterChanged: () => _communicationService.Configure(IpAddress, Port ?? 0));
        }

        public int? Port
        {
            get => Get<int?>();
            set => Set(value, afterChanged: () => _communicationService.Configure(IpAddress, Port ?? 0));
        }

        public ICommand MoveUpCommand => DefaultCommand(_communicationService.MoveUpAsync);

        public ICommand MoveDownCommand => DefaultCommand(_communicationService.MoveDownAsync);

        public ICommand MoveLeftCommand => DefaultCommand(_communicationService.MoveLeftAsync);

        public ICommand MoveRightCommand => DefaultCommand(_communicationService.MoveRightAsync);

        public RemoteViewModel()
        {
            _communicationService = new UdpCommunicationService();
        }
    }
}