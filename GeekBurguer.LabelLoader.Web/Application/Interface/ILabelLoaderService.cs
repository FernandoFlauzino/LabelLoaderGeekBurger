using GeekBurguer.LabelLoader.Web.Application.Request.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurguer.LabelLoader.Web.Application.Interface
{
    public interface ILabelLoaderService
    {
        Task<CreateIngredientsRequest> ReadImageVisonService();
    }
}
