﻿using DemoServerless.Entities;

namespace DemoServerless.Models.User
{
    public class UserResponse: CommonModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
