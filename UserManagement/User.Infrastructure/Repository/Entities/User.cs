using System.Collections.Generic;
using System.Threading.Tasks;

namespace User.Infrastructure.Repository.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool LoggedOn { get; set; }
    }

    public class Users
    {
        public List<User> Accounts { get; set; }
    }
}
