using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Test_API.Models.Orders.DbDTO;

namespace Test_API.Models.Orders.Api
{
    [SwaggerSchema("Product", Format = "object", Title = "line")]
    public class ProductApiDTO
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "qty")]
        public int Qty { get; set; }
    }
}
