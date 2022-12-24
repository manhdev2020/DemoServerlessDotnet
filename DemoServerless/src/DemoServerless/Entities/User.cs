using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoServerless.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsVerify { get; set; }
        public Role Role { get; set; }
        public virtual Profile? Profile { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role != Role.Admin && Role != Role.User)
            {
                yield return new ValidationResult("Role không hợp lệ.", new[] { "Role" });
            }
        }
    }
}
