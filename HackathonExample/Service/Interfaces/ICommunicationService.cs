using System.Threading.Tasks;

namespace HackathonExample.Service.Interfaces
{
    public interface ICommunicationService
    {
        void Configure(string ipAddress, int port);

        Task MoveUpAsync();

        Task MoveDownAsync();

        Task MoveLeftAsync();

        Task MoveRightAsync();
    }
}