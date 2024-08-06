using Application.Models.Product;

namespace Application.Models.Order;

public class DetailOrderInfo : BasicOrderInfo
{
    public string Address { get; set; }
    public List<ChauPhatAluminium.Entities.Order.Trace> Traces { get; set; }
    public HashSet<OrderProduct> Details { get; set; }
    public class OrderProduct
    {
        public MinimalProductInfo Product { get; set; }
        public int Quantity { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public decimal Total { get; set; }
    }
}