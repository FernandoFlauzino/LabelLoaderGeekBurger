using GeekBurger.Ingredients.Contract.Response;
using GeekBurguer.LabelLoader.Web.Application.Interface.Api;
using GeekBurguer.LabelLoader.Web.Application.Request.Api;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurguer.LabelLoader.Web.Infra.Repository
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly IConfiguration config;

        public IngredientsRepository()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        }
        public async Task<bool> CreateIngredients(IngredientsToUpsert request)
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
