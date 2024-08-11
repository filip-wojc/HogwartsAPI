namespace HogwartsAPI.Dtos.UserDtos
{
    public class ModifyUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
