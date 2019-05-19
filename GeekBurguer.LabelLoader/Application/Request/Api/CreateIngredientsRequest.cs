using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurguer.LabelLoader.Application.Request.Api
{
    public class CreateIngredientsRequest
    {
        public string ItemName { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
