namespace Cart.Domain.Models
{
    public class ItemInCart
    {
        public Item Item { get; set; }
        public decimal TotalPrice { get; set; }
        public int QuantityInCart { get; set; }      
    }
}
