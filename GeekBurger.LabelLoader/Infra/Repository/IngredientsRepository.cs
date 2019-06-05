using GeekBurger.LabelLoader.Application.Interface.Api;
using GeekBurger.LabelLoader.Application.Request.Api;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Infra.Repository
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly IConfiguration config;

        public IngredientsRepository()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        }
        public async Task<bool> CreateIngredients(CreateIngredientsRequest request)
        {
            var urlApi = config["API:IngredientsUrl"];

            using (HttpClient client = new HttpClient())
            {
                var stringData = JsonConvert.SerializeObject(request);
                var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(urlApi, contentData).Result;
                string responseData = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<bool>(responseData);
            }

        }
    }
}
