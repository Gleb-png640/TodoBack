namespace TodoBack.Dtos.Users
{
    public class TokenResponeDto
    {
        public required string AccesToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
