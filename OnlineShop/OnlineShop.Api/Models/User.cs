﻿using System;

namespace OnlineShop.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool LoggedOn { get; set; }
    }
}