namespace HogwartsAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PasswordHash { get; set; }
        public virtual Role? Role { get; set; }
        public int RoleId { get; set; }
    }
}
