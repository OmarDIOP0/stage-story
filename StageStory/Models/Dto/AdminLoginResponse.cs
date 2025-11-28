namespace StageStory.Models.Dto
{
    public class AdminLoginResponse
    {
        public string? Token { get; set; }
        public DateTime TokenExpireAt { get; set; }
        public string? RefreshToken { get; set; }
        public Admin? Admin { get; set; }
    }
}
