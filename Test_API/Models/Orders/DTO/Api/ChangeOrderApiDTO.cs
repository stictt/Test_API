using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Test_API.Models.Orders.Api;

namespace Test_API.Models.Orders.DTO.Api
{
    [SwaggerSchema("Change", Format = "object", Title = "ChangeOrder")]
    public class ChangeOrderApiDTO
    {

        [EnumDataType(typeof(OrderStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public OrderStatus Status { get; set; }

        [JsonProperty(PropertyName = "lines")]
        [System.Text.Json.Serialization.JsonPropertyName("lines")]
        public List<ProductApiDTO> Products { get; set; }
    }
}
