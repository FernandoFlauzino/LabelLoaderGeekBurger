using GeekBurger.Ingredients.Contract.Response;
using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Application.Interface.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

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
        private readonly ILababelLoaderChangedService _lababelLoaderChangedService;


        public LabelLoaderService(IConfiguration configuration,
            IIngredientsRepository ingredientsRepository, ILababelLoaderChangedService lababelLoaderChangedService)
        {
            _configuration = configuration;
            _ingredientsRepository = ingredientsRepository;
            _lababelLoaderChangedService = lababelLoaderChangedService;
        }

        /// <summary>
        /// Faz a leitura dos ingredientes na imagem e 
        /// envia para a fila 
        /// </summary>
        /// <param name="pathImage"></param>
        public void ReadImageVisonService(string pathImage)
        {
            OcrResults results;

            var visionServiceClient = new VisionServiceClient(_configuration["API:VisionAPIKey"], _configuration["API:VisionUrl"]);

            var imageByte = DownloadImageFromHttp(pathImage);
            var memStream = new MemoryStream(imageByte);
            memStream.Seek(0, SeekOrigin.Begin);

            results =  visionServiceClient.RecognizeTextAsync(memStream).Result;

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
                ProductId = Guid.NewGuid(),
                Ingredients = new List<string>()
            };

            wordsSplitByComma.Distinct().ToList()
            .ForEach(wordText =>
            {
                var text = wordText.Trim();
                if (!String.IsNullOrWhiteSpace(text))
                    request.Ingredients.Add(text);
            });

            _lababelLoaderChangedService
                .AddToMessageList(request);

            _lababelLoaderChangedService.SendMessagesAsync();
        }

        public byte[] DownloadImageFromHttp(string url)
        {
            byte[] image;
            using (WebClient client = new WebClient())
            {
                image = client.DownloadData(new Uri(url, UriKind.Absolute));
            }

            return image;
        }
    }
}
