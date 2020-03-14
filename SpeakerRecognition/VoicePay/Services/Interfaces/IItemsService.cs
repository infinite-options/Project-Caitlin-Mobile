using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoicePay.Models;

namespace VoicePay.Services.Interfaces
{
    public interface IItemsService
    {
        Task<List<Item>> GetAll();
        Task<List<Item>> GetByCategory(string categoryName);
    }
}
