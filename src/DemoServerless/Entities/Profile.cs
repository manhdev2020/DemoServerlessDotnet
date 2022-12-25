using System;
using System.Collections.Generic;

namespace DemoServerless.Entities
{
    public partial class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Country { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
