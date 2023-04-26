using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Test_API.Infrastructure;
using Test_API.Models.Orders.DbDTO;

namespace Test_API.Models.Orders.Api
{
    [SwaggerSchema("Create", Format = "object", Title = "CreateOrder")]
    public class CreateOrderApiDTO
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "lines")]
        [System.Text.Json.Serialization.JsonPropertyName("lines")]
        public List<ProductApiDTO> Products { get; set; }
    }
}
