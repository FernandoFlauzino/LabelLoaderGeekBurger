using GeekBurger.Ingredients.Contract.Request;
using GeekBurger.Ingredients.Contract.Response;
using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Application.Interface.Api;
using GeekBurger.LabelLoader.Web.Application.Request.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Web.Application.Service
{
    public class LabelLoaderService : ILabelLoaderService
    {
        public static string[] blacklist = new string[]
         {
             "ingredients",
             "processed in a facility thathandles",
             "products" ,
             "allergens" ,
             "contains"
         };

        public const string AndWithSpace = " and ";
        public const string CommaWithSpace = " , ";
        private const string Exit = "exit";
        private readonly IConfiguration _configuration;
        private readonly IIngredientsRepository _ingredientsRepository;

        public LabelLoaderService(IConfiguration configuration,
            IIngredientsRepository ingredientsRepository)
        {
            _configuration = configuration;
            _ingredientsRepository = ingredientsRepository;
        }

        public async Task<bool> ReadImageVisonService(string base64EncodedData)
        {
            //var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            //var pathImage = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return await Task.FromResult(false);
            }

            OcrResults results;

            var visionServiceClient = new VisionServiceClient(_configuration["VisionAPIKey"], _configuration["VisionUrl"]);

            var name = File.ReadAllBytes(base64EncodedData);

            using (Stream imageFileStream = new MemoryStream(name))
            {
                results = visionServiceClient.RecognizeTextAsync(imageFileStream).Result;
            }

            //using (Stream imageFileStream = File.OpenRead(pathImage))
            //{
            //    results = visionServiceClient.RecognizeTextAsync(imageFileStream).Result;
            //}

            var lines = results.Regions.SelectMany(region => region.Lines);
            var words = lines.SelectMany(line => line.Words);
            var wordsText = words.Select(word => word.Text.ToUpper());

            var wordsJoint =
                string.Join(' ', wordsText).Replace(AndWithSpace, CommaWithSpace, StringComparison.InvariantCultureIgnoreCase);

            foreach (var item in blacklist)
            {
                wordsJoint = wordsJoint.Replace(item, ",",
                StringComparison.InvariantCultureIgnoreCase);
            }

            var wordsSplitByComma = wordsJoint.Split(',').ToList();

            var request = new IngredientsToUpsert
            {
                //TODO: Confirmar se precisa enviar o ID
                ProductId = 1
            };


            //Ingredients
            wordsSplitByComma.Distinct().ToList()
            .ForEach(wordText =>
            {
                var text = wordText.Trim();
                if (!String.IsNullOrWhiteSpace(text))
                    request.Ingredients.Add(text);
            });

            //chamar a api que vai inserir os ingredientes
            var result = await _ingredientsRepository.CreateIngredients(request);

            if (result)
            {
                //TODO: incluir tratamento para renomear o arquivo
            }

            return await Task.FromResult(result);

        }
    }
}
