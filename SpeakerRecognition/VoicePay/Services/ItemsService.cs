using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoicePay.Models;
using VoicePay.Services.Interfaces;

namespace VoicePay.Services
{
    public class ItemsService : IItemsService
    {
        private List<Item> _items = new List<Item>();

        public ItemsService()
        {
            Init();
        }

        private void Init()
        {
            _items = new List<Item>
            {
                new Item { Name = "Macbook Pro 13\"", Description = "", Price = 1500, Category = "Laptops" },
                new Item { Name = "Macbook Pro 15\"", Description = "", Price = 2200, Category = "Laptops" },
                new Item { Name = "Macbook Air 13\"", Description = "", Price = 1000, Category = "Laptops" },
                new Item { Name = "Macbook Air 15\"", Description = "", Price = 1300, Category = "Laptops" },
                new Item { Name = "Macbook Pro 13\" Touchbar", Description = "", Price = 2200, Category = "Laptops" },
                new Item { Name = "Macbook Pro 15\" Touchbar", Description = "", Price = 2500, Category = "Laptops" },

                new Item { ImageUri = "tablet.png", Name = "iPad Pro", Description = "", Price = 700, Category = "Tablets" },
                new Item { ImageUri = "tablet.png", Name = "iPad Mini", Description = "", Price = 500, Category = "Tablets" },
                new Item { ImageUri = "tablet.png", Name = "Samsung Tablet 7\"", Description = "", Price = 400, Category = "Tablets" },
                new Item { ImageUri = "tablet.png", Name = "LG Tablet 8.2\"", Description = "", Price = 350, Category = "Tablets" },

                new Item { ImageUri = "phone.png", Name = "iPhone 6", Description = "", Price = 650, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "iPhone 6S Plus", Description = "", Price = 900, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "iPhone 7", Description = "", Price = 1100, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "LG G6", Description = "", Price = 700, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "Galaxy Note 8", Description = "", Price = 850, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "Moto G6", Description = "", Price = 500, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "Huawei P10", Description = "", Price = 600, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "Huawei P9", Description = "", Price = 450, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "iPhone 8", Description = "", Price = 1800, Category = "Cellphones" },
                new Item { ImageUri = "phone.png", Name = "LG G4 Beat", Description = "", Price = 300, Category = "Cellphones" },

                new Item { ImageUri = "adapter.png", Name = "HDMI to VGA", Price = 15, Category = "Adapters" },
                new Item { ImageUri = "adapter.png", Name = "MiniDisplayPort to VGA", Price = 25, Category = "Adapters" },
                new Item { ImageUri = "adapter.png", Name = "USB to VGA", Price = 10, Category = "Adapters" },
                new Item { ImageUri = "adapter.png", Name = "USB Type C to Type A", Price = 12, Category = "Adapters" }
            };
        }

        public async Task<List<Item>> GetAll()
        {
            return _items;
        }

        public async Task<List<Item>> GetByCategory(string categoryName)
        {
            return _items.Where(i => i.Category == categoryName).ToList();
        }
    }
}
