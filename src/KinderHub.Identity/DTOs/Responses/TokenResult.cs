namespace KinderHub.Identity.DTOs.Responses
{
    public class TokenResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}