using Microsoft.AspNetCore.Mvc;

namespace DrinkingPassion.Api.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : DrinkingPassion.Api.Controllers.BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new DrinkingPassion.Api.Errors.ApiResponse(code));
        }
    }
}
