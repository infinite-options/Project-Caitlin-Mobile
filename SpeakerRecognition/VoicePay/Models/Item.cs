namespace VoicePay.Models
{
    public class Item
    {
        public Item()
        {
            ImageUri = "laptop_mac.png";
        }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUri { get; set; }
    }
}
