using DemoServerless.Entities;
using System.Text.Json.Serialization;

namespace DemoServerless.Models.User
{
    public class LoginResponse: CommonModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
