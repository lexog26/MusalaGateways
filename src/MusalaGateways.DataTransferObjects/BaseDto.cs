using Newtonsoft.Json;
using System;

namespace MusalaGateways.DataTransferObjects
{
    public class BaseDto<T>
    {
        [JsonProperty(PropertyName = "id")]
        public T Id { get; set; }
    }
}
