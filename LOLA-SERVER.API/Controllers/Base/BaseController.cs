using LOLA_SERVER.API.Utils.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOLA_SERVER.API.Controllers.Base
{

    public abstract class BaseController : ControllerBase
    {

        protected IActionResult ApiResponse<T>(bool success, string message, T data = default)
        {
            return Ok(new ApiResponse<T>(success, message, data));
        }

        protected IActionResult ApiResponse<T>(T data)
        {
            return ApiResponse(true, "Request succeeded", data);
        }

        protected IActionResult ApiResponseError(string message)
        {
            return BadRequest(new ApiResponse<object>(false, message));
        }

        protected IActionResult ApiResponseServerError(string message)
        {
            return StatusCode(500, new ApiResponse<object>(false, message));
        }

    }
}
