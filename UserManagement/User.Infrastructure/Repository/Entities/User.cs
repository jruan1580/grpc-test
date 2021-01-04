using System;
using System.Collections.Generic;

namespace User.Infrastructure.Repository.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public bool LoggedOn { get; set; }
    }

    public class Users
    {
        public List<User> Accounts { get; set; }
    }
}
