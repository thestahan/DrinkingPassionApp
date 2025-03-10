using Microsoft.AspNetCore.Mvc;

namespace DrinkingPassion.Api.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new Errors.ApiErrorResponse(code));
        }
    }
}
