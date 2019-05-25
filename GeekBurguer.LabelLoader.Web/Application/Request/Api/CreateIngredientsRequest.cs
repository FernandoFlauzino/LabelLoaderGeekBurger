using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurguer.LabelLoader.Web.Application.Request.Api
{
    public class CreateIngredientsRequest
    {
        public string ItemName { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
