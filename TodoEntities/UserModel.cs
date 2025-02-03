
namespace TodoEntities
{
    public class Usermodel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsNewlyRegistered { get; set; } = false;
    }
}
