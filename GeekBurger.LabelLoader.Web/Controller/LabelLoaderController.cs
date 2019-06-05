using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Application.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.LabelLoader.Web.Controller
{
    [Produces("application/json")]
    [Route("api/LabelLoader")]
    public class LabelLoaderController : ControllerBase
    {
        private readonly ILabelLoaderService _labelLoaderService;

        public LabelLoaderController(ILabelLoaderService labelLoaderService)
        {
            _labelLoaderService = labelLoaderService;
        }

 
        [HttpPost]
        public IActionResult ReadImage([FromBody] ReadRequest request)
        {
            try
            {
                var result = _labelLoaderService.ReadImageVisonService(request.PathImage).Result;
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}