using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
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

    }
}
