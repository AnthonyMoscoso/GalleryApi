namespace Model.Dto.User
{
    public class UserInputDto
    {

        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Base64 { get; set; }
    }
}
