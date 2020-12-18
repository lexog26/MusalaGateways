using MusalaGateways.DataTransferObjects.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogic.Interfaces
{
    public interface IGatewayService : ITransactionalService<GatewayDto, int>
    {
        /// <summary>
        /// Returns gateway's devices
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDto>> GetGatewayDevicesAsync(int gatewayId);
    }
}
