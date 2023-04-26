using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Test_API.Infrastructure;
using Test_API.Models.Orders.DbDTO;

namespace Test_API.Models.Orders.Api
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
