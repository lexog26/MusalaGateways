using MusalaGateways.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.Domain.Entities
{
    public class Device : Entity<int>
    {
        public int Uid { get; set; }

        public string Vendor { get; set; }

        public DeviceStatus Status { get; set; }

        public int GatewayId { get; set; }

        public Gateway Gateway { get; set; }
    }
}
