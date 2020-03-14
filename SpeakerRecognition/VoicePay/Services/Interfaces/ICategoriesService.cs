using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoicePay.Models;

namespace VoicePay.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<Category>> GetAll();
    }
}
