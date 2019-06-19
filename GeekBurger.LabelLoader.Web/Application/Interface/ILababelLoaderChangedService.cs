using GeekBurger.Ingredients.Contract.Response;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace GeekBurger.LabelLoader.Web.Application.Interface
{
    public interface ILababelLoaderChangedService : IHostedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IngredientsToUpsert changes);
    }
}
