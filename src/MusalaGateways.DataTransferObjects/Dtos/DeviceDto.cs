using MusalaGateways.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.DataTransferObjects.Dtos
{
    public class DeviceDto : BaseDto<int>
    {
        [JsonProperty(PropertyName = "uid")]
        public int Uid { get; set; }

        [JsonProperty(PropertyName = "vendor")]
        public string Vendor { get; set; }

        [JsonProperty(PropertyName = "status")]
        public DeviceStatus Status { get; set; }

        [JsonProperty(PropertyName = "gateway_id")]
        public int GatewayId { get; set; }

    }
}
