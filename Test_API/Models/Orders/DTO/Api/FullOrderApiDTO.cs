using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using Test_API.Models.Orders.Api;

namespace Test_API.Models.Orders.DTO.Api
{
    [SwaggerSchema("Order", Format = "object", Title = "Order")]
    public class FullOrderApiDTO
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public OrderStatus Status { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [SwaggerSchema("lines", Format = "Array")]
        [JsonProperty(PropertyName = "lines")]
        [System.Text.Json.Serialization.JsonPropertyName("lines")]
        public List<ProductApiDTO> Products { get; set; }
    }
}
