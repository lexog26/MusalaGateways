using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MusalaGateways.BusinessLogic.Services
{
    public class DeviceService : TransactionalService<DeviceDto, Device, int>, IDeviceService
    {
        public DeviceService(IRepository repository,
                             IMapper mapper,
                             IUnitOfWork unitOfWork) : base(mapper, unitOfWork, repository)
        { }

        public override async Task<DeviceDto> CreateAsync(DeviceDto dto)
        {
            var gateway = await _repository.GetEntityByIdAsync<Gateway, int>(dto.GatewayId);
            if(gateway.Devices.Count() == 10)
            {
                throw new ArgumentOutOfRangeException($"Max number of devices for this gateway: {dto.GatewayId}");
            }
            return await base.CreateAsync(dto);
        }
    }
}
