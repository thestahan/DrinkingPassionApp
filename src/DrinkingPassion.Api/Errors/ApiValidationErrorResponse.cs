using System.Collections.Generic;

namespace DrinkingPassion.Api.Errors
{
    public class ApiValidationErrorResponse : ApiErrorResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public required IEnumerable<string> Errors { get; set; }
    }
}
