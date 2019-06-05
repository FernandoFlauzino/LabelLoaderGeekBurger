using GeekBurger.LabelLoader.Web.Application.Request.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Web.Application.Interface
{
    public interface ILabelLoaderService
    {
        Task<bool> ReadImageVisonService(string pathImage);
    }
}
