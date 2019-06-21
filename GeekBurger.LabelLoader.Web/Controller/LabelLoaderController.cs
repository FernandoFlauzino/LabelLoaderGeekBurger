using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Application.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

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
                request.PathImage = @"https://functionburgera06d.blob.core.windows.net/processar-new/download.png";
                _labelLoaderService.ReadImageVisonService(request.PathImage);

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}