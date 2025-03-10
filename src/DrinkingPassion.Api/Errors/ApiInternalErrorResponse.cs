namespace DrinkingPassion.Api.Errors
{
    public class ApiInternalErrorResponse : ApiErrorResponse
    {
        public ApiInternalErrorResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
