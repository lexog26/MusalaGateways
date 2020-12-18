using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;

namespace MusalaGateways.BusinessLogic.Services
{
    public class DeviceService : TransactionalService<DeviceDto, Device, int>, IDeviceService
    {
        public DeviceService(IRepository repository,
                             IMapper mapper,
                             IUnitOfWork unitOfWork) : base(mapper, unitOfWork, repository)
        { }

    }
}
