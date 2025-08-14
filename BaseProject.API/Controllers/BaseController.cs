using BaseProject.Application.DTOs;
using BaseProject.Shared.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class BaseController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test");
        }
        protected IActionResult Success<T>(T data, string message = "Success")
            => Ok(ResponseDto<T>.SuccessResponse(data, message));
    }
}
