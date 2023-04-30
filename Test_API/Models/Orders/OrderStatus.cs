using Swashbuckle.AspNetCore.Annotations;

namespace Test_API.Models.Orders
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
