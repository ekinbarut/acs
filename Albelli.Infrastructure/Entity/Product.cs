using Albelli.Abstraction.Enum;
using Albelli.Abstraction.Infrastructure;

namespace Albelli.Domain.Entity
{
    public class Product
    {
        public ProductType ProductType { get; set; }
        public int Quantity { get; set; }
        
    }
}