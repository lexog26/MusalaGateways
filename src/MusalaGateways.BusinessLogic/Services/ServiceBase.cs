using AutoMapper;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.BusinessLogic.Interfaces;

namespace MusalaGateways.BusinessLogic.Services
{
    public abstract class ServiceBase : IServiceBase 
    {
        protected readonly IMapper _mapper;
        protected readonly IRepository _repository;

        public ServiceBase(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}
