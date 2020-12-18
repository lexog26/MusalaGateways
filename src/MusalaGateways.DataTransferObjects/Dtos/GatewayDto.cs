using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.DataTransferObjects.Dtos
{
    public class GatewayDto : BaseDto<int>
    {
        [JsonProperty(PropertyName = "serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ipv4_address")]
        public string Ipv4Address { get; set; }

        [JsonProperty(PropertyName = "device_ids")]
        public IEnumerable<int> DevicesIds { get; set; }
    }
}
