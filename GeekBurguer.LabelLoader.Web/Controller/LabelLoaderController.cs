using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurguer.LabelLoader.Web.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurguer.LabelLoader.Web.Controller
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
        public IActionResult ReadImage()
        {
            try
            {
                var result = _labelLoaderService.ReadImageVisonService().Result;
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}