using System;
namespace VoicePay.Models
{
    public class CartItem : Item
    {
        public int Quantity { get; set; }
        public double Total => Price * Quantity;

        public CartItem(Item item)
        {
            base.ImageUri = item.ImageUri;
            base.Category = item.Category;
            base.Name = item.Name;
            base.Price = item.Price;
            base.Description = item.Description;
        }
    }
}
