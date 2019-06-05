using GeekBurger.Ingredients.Contract.Response;
using GeekBurger.LabelLoader.Web.Application.Request.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Web.Application.Interface.Api
{
    public interface IIngredientsRepository
    {
        Task<bool> CreateIngredients(IngredientsToUpsert request);
    }
}
