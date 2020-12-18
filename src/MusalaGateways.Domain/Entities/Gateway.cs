using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.Domain.Entities
{
    public class Gateway : Entity<int>
    {
        public string SerialNumber { get; set; }

        public string Name { get; set; }

        public string Ipv4Address { get; set; }

        public IEnumerable<Device> Devices { get; set; }
    }
}
