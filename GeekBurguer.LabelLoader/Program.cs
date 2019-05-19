using GeekBurguer.LabelLoader.Application.Interface.Api;
using GeekBurguer.LabelLoader.Application.Request.Api;
using GeekBurguer.LabelLoader.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GeekBurguer.LabelLoader
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        

        static void Main(string[] args)
        {
            RegisterServices();
            CreateIngredients();
            DisposeServices();
        }


        private static void CreateIngredients()
        {
            var service = _serviceProvider.GetService<IIngredientsRepository>();

            var ingredientsList = new List<string>();

            string[] ingredients = { "diary", "gluten","soy"};

            ingredientsList.AddRange(ingredients);

            var ingredientsquest = new CreateIngredientsRequest
            {
                Ingredients = ingredientsList,
                ItemName = "Teste nome ingrediente"
            };


            service.CreateIngredients(ingredientsquest);
        }


        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddScoped<IIngredientsRepository, IngredientsRepository>();

            
            _serviceProvider = collection.BuildServiceProvider();

           
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
