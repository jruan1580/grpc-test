using System;

namespace User.Domain.Model
{
    public class CoreUser
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public bool LoggedOn { get; set; }
    }
}
