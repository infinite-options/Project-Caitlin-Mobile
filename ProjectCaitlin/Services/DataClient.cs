using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCaitlin.Services
{
    public interface IDataClient
    {
        Task<user> GetUser(string userId);

        Task<List<user>> GetAllOtherTAs(string userId);

        Task<List<GratisObject>> GetGoalsAndRoutines(string userId);

        Task<string> GetUserId(string emailId);

        Task<List<atObject>> GetActionsAndTasks(string grId);

        Task<List<atObject>> GetActionsAndTasks(string grId, string type);
    }
}
