using Newtonsoft.Json;
using ProjectCaitlin.Config;
using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCaitlin.Services.Rds
{
    class RdsClient : IDataClient
    {
        private readonly string BaseUrl;

        public RdsClient(string BaseUrl)
        {
            this.BaseUrl = BaseUrl;
        }

        public async Task<user> GetUser(string userId)
        {
            string url = BaseUrl + RdsConfig.aboutMeUrl + "/" + userId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                UsersResponse usersResponse = JsonConvert.DeserializeObject<UsersResponse>(response.Result);
                Console.WriteLine("----------------Some Converted--------------------");
                Console.WriteLine(usersResponse.message);
                Console.WriteLine(usersResponse.result[0].user_first_name);
                Console.WriteLine(usersResponse.result[0].user_last_name);
                Console.WriteLine("----------------Unconverted--------------------");
                Console.WriteLine(response.Result.ToString());

                return usersResponse.ToUser(userId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task<List<user>> GetAllOtherTAs(string userId)
        {
            string url = BaseUrl + RdsConfig.listAllOtherTA + "/" +userId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                UsersResponse usersResponse = JsonConvert.DeserializeObject<UsersResponse>(response.Result);
                return usersResponse.ToUsersList();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task<List<GratisObject>> GetGoalsAndRoutines(string userId)
        {
            string url = BaseUrl + RdsConfig.goalsAndRoutinesUrl + "/" + userId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                GratisResponse gratisResponse = JsonConvert.DeserializeObject<GratisResponse>(response.Result);
                return gratisResponse.ToGratisList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }


        public async Task<string> GetUserId(string emailId)
        {
            string url = BaseUrl + RdsConfig.UserIdFromEmailUrl + "/" + emailId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                EmailToUserIdResponse userIdResponse = JsonConvert.DeserializeObject<EmailToUserIdResponse>(response.Result);
                return userIdResponse.result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }

        public async Task<List<atObject>> GetActionsAndTasks(string grId)
        {
            string url = BaseUrl + RdsConfig.actionAndTaskUrl + "/" + grId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                ActionTaskResponse actionAndTaskResponse = JsonConvert.DeserializeObject<ActionTaskResponse>(response.Result);
                return actionAndTaskResponse.ToActionTaskList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }


        public async Task<List<atObject>> GetActionsAndTasks(string grId, string type)
        {
            string url = BaseUrl + RdsConfig.actionAndTaskUrl + "/" + grId;
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url);
                response.Wait();
                ActionTaskResponse actionAndTaskResponse = JsonConvert.DeserializeObject<ActionTaskResponse>(response.Result);
                return actionAndTaskResponse.ToActionTaskList(type);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }

    }
}
