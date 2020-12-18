using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using System.Linq;

namespace MusalaGateways.BusinessLogic.Configurations.Mapper
{
    public class MusalaMapperProfile : MapperProfileBase
    {
        public MusalaMapperProfile()
        {
            CreateMapperConfig();
        }

        /// <summary>
        /// Maps entities <---> dtos
        /// </summary>
        internal void CreateMapperConfig()
        {
            //Gateway to GatewayDto, GatewayDto to Gateway
            CreateMap<Gateway, GatewayDto>()
                .ForMember(dto => dto.DevicesIds, map => map.MapFrom(org => org.Devices.Select(x => x.Id)))
                .ReverseMap();

            //Device to DeviceDto, DeviceDto to Device
            CreateMap<Device, DeviceDto>()
                .ReverseMap();
        }
    }
}
