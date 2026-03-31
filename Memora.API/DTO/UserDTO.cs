namespace SimpleAUTH.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public DateTime LastLoginAt { get; set; } = DateTime.Now;
    }
}
