using Store.Domain.Entities.Commons;

namespace Store.Domain.Entities.Products
{
    public class ProductFetures : BaseEntity
    {
        public virtual Product Product { get; set; }
        public long ProductId { get; set; }
        public string DisplayName { get; set; }
        public string value { get; set; }
    }
}
