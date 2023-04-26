using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;

namespace Test_API.Infrastructure
{
    [SwaggerSchema("enum", Format = "object", Title = "Status")]
    public enum OrderStatus
    {
        Paid,
        New,
        ExpectedPayment,
        HandedForDelivery,
        Delivered,
        Completed
    }
}
