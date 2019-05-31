using GeekBurger.Ingredients.Contract.Response;
using GeekBurguer.LabelLoader.Web.Application.Request.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurguer.LabelLoader.Web.Application.Interface.Api
{
    public interface IIngredientsRepository
    {
        Task<bool> CreateIngredients(IngredientsToUpsert request);
    }
}
