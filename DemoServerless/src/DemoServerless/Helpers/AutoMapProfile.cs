using DemoServerless.Entities;
using DemoServerless.Models.User;

namespace DemoServerless.Helpers
{
    public class AutoMapProfile: AutoMapper.Profile
    {
        public AutoMapProfile() {
            CreateMap<User, LoginRequest>();
            CreateMap<User, LoginResponse>();
            CreateMap<User, RegisterRequest>();
            CreateMap<User, RegisterResponse>();
            CreateMap<User, UserResponse>();

            CreateMap<RegisterRequest, User>();
            CreateMap<RegisterResponse, User>();
            CreateMap<LoginRequest, User>();
            CreateMap<LoginResponse, User>();
            CreateMap<UserResponse, User>();

            CreateMap<List<User>, UserResponse>();
            CreateMap<List<UserResponse>, User>();
        }
    }
}
