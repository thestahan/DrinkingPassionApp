namespace API.Dtos.Accounts
{
    public class ChangeForgottenPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
