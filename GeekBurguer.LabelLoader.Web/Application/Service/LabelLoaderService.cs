using GeekBurguer.LabelLoader.Web.Application.Interface;
using GeekBurguer.LabelLoader.Web.Application.Interface.Api;
using GeekBurguer.LabelLoader.Web.Application.Request.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurguer.LabelLoader.Web.Application.Service
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

        public async Task<bool> ReadImageVisonService()
        {

            var templateImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "hamburger.jpg");

            OcrResults results;

            var visionServiceClient = new VisionServiceClient(_configuration["VisionAPIKey"], _configuration["VisionUrl"]);

            using (Stream imageFileStream = File.OpenRead(templateImage))
            {
                results = visionServiceClient.RecognizeTextAsync(imageFileStream).Result;
            }

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

            var request = new CreateIngredientsRequest
            {
                ItemName = "Item Name"
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
