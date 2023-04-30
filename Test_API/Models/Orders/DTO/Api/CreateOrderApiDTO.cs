using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

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
