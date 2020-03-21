using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectCaitlin.Services
{
    public class FirebaseFunctionsService
    {
        public async Task<bool> GRUserNotificationSetToTrue(string routineId, string routineIdx)
        {
            try
            {
                Console.WriteLine("stuck here2");
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/GRUserNotificationSetToTrue"),
                    Method = HttpMethod.Post
                };

                //Format Headers of Request
                request.Headers.Add("userId", App.User.id);
                request.Headers.Add("routineId", routineId);
                request.Headers.Add("routineNumber", routineIdx);
                var client = new HttpClient();

                // without async, will get stuck, needs bug fix
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStep(string routineId, string taskId, string stepNumber)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteInstructionOrStep"),
                Method = HttpMethod.Post
            };

            //Format Headers of Request with included Token
            request.Headers.Add("userId", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("taskId", taskId);
            request.Headers.Add("stepNumber", stepNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CompleteRoutine(string routineId, string routineIdx)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteGoalOrRoutine");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("routineNumber", routineIdx);

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateTask(string routineId, string taskId, string taskIndex)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteActionOrTask");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("taskId", taskId);
            request.Headers.Add("taskNumber", taskIndex);

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateInstruction(string goalId, string actionId, string instructionNumber)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteInstructionOrStep");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("routineId", goalId);
            request.Headers.Add("taskId", actionId);
            request.Headers.Add("stepNumber", instructionNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
