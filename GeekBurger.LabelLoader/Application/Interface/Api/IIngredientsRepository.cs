using GeekBurger.LabelLoader.Application.Request.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Application.Interface.Api
{
    public interface IIngredientsRepository
    {
        Task<bool>CreateIngredients(CreateIngredientsRequest request);
    }
}
