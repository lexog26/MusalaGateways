using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogic.Services
{
    public class GatewayService : TransactionalService<GatewayDto, Gateway, int>, IGatewayService
    {
        public GatewayService(IRepository repository,
                              IMapper mapper,
                              IUnitOfWork unitOfWork) : base(mapper, unitOfWork, repository)
        { }

        public override Task<GatewayDto> CreateAsync(GatewayDto dto)
        {
            IPAddress address;
            if (!IPAddress.TryParse(dto.Ipv4Address, out address))
            {
                throw new InvalidOperationException("Invalid ipv4 address");
            }
            dto.DevicesIds = new List<int>();
            return base.CreateAsync(dto);
        }

        public async Task<IEnumerable<DeviceDto>> GetGatewayDevicesAsync(int gatewayId)
        {
            var gateway = await _repository.GetEntityByIdAsync<Gateway, int>(gatewayId, includeProperties: "Devices");
            if(gateway == null)
            {
                throw new KeyNotFoundException("Invalid gatewayId");
            }
            return _mapper.Map<IEnumerable<DeviceDto>>(gateway.Devices);
        }

        public override async Task<IEnumerable<GatewayDto>> GetAllAsync(int limit = int.MaxValue)
        {
            var gateways = await base.GetAllAsync(limit);
            var dtoIds = gateways.Select(x => x.Id).ToList();

            //Request Devices ids without load full Devices entities
            var ids = await _repository.GetByFilterAsync<Device, KeyValuePair<int, int>>(
                filter: x => dtoIds.Contains(x.GatewayId),
                selector: x => new KeyValuePair<int, int>(x.GatewayId, x.Id)
                );
            foreach (var gateway in gateways)
            {
                gateway.DevicesIds = ids.Where(x => x.Key == gateway.Id).Select(x => x.Value);
            }
            return gateways;
        }

        public async override Task<GatewayDto> GetByIdAsync(int id)
        {
            var gateway = await base.GetByIdAsync(id);
            if (gateway != null)
            {
                gateway.DevicesIds = await _repository.GetByFilterAsync<Device, int>(
                    filter: x => x.GatewayId == id,
                    selector: x => x.Id
                    );
            }
            return gateway;
        }
    }
}
