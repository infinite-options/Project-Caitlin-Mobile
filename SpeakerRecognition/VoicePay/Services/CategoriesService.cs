using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoicePay.Models;
using VoicePay.Services.Interfaces;

namespace VoicePay.Services
{
    public class CategoriesService : ICategoriesService
    {
        private List<Category> _categories = new List<Category>();

        public CategoriesService()
        {
            Init();
        }

        private void Init()
        {
            _categories = new List<Category>
            {
                new Category { Description = "Laptops", ImageUri = "laptop_mac.png" },
                new Category { Description = "Cellphones", ImageUri="phone.png" },
                new Category { Description = "Tablets", ImageUri = "tablet.png" },
                new Category { Description = "Adapters", ImageUri = "adapter.png" }
            };
        }

        public async Task<List<Category>> GetAll()
        {
            return _categories;
        }
    }
}
