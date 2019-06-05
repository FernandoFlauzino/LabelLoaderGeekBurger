using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.LabelLoader.Application.Request.Api
{
    public class CreateIngredientsRequest
    {
        public string ItemName { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
