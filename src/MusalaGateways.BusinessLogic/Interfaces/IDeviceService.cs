using MusalaGateways.DataTransferObjects.Dtos;

namespace MusalaGateways.BusinessLogic.Interfaces
{
    public interface IDeviceService : ITransactionalService<DeviceDto, int>
    {
    }
}
