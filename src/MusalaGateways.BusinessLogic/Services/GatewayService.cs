using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
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
            return base.CreateAsync(dto);
        }

        public async Task<IEnumerable<DeviceDto>> GetGatewayDevicesAsync(int gatewayId)
        {
            var gateway = await _repository.GetEntityByIdAsync<Gateway, int>(gatewayId);
            return _mapper.Map<IEnumerable<DeviceDto>>(gateway.Devices);
        }
    }
}
