using System.Collections.Generic;

namespace DrinkingPassion.Api.Errors
{
    public class ApiValidationErrorResponse : DrinkingPassion.Api.Errors.ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
