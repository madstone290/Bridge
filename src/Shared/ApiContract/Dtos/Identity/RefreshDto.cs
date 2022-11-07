namespace Bridge.Shared.ApiContract.Dtos.Identity
{
    public class RefreshDto
    {
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
