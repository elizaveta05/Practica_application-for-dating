using makets.Model.Model_users;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
namespace makets.helper
{
    class APIService
    {
        public async static Task<List<DataUser>?> GetUsers()
        {

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, policy) => true
            });

            httpClient.BaseAddress = new Uri("https://localhost:7036");

            var response = await httpClient.GetAsync("/users/getUsers");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<List<DataUser>?>(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                return null;
            }
        }

        public async static Task<string?> GetUserDescription(int userId)
        {
            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, policy) => true
            });

            httpClient.BaseAddress = new Uri("https://localhost:7036");
            var response = await httpClient.PostAsJsonAsync("/Profile/getUserDescription", new
            {
                userId = userId
            });
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            try
            {
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return "";
            }
        }
    }
}
