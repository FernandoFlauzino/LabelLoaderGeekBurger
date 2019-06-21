using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                var photo = @"https://functionburgera06d.blob.core.windows.net/processar-new/download.png";
                _labelLoaderService.ReadImageVisonService(photo);
                //var result = _labelLoaderService.ReadImageVisonService(request.PathImage).Result;
                return Ok();
            }
            catch(Exception ex)
            {
                
                return  StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}