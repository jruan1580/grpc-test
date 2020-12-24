namespace User.Domain.Model
{
    public class CoreUser
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool LoggedOn { get; set; }
    }
}
