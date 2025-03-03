namespace DrinkingPassion.Api.Errors
{
    public class ApiException : DrinkingPassion.Api.Errors.ApiResponse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}
