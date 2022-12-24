using AutoMapper;
using DemoServerless.Configs;
using DemoServerless.Entities;
using DemoServerless.Filters;
using DemoServerless.Models.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoServerless.Services
{
    public interface IAuthService
    {
        RegisterResponse Register(RegisterRequest registerRequest);
        LoginResponse Login(LoginRequest loginRequest);
        bool ChangePassword(int userId, ChangePassword changePassword);
    }
    public class AuthService : IAuthService
    {
        private readonly CoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppConfigs _appConfigs;

        public AuthService(CoreDbContext context, IMapper mapper, IOptions<AppConfigs> appConfigs)
        {
            _context = context;
            _mapper = mapper;
            _appConfigs = appConfigs.Value;
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            var checkUserExist = _context.Users.SingleOrDefault(u => u.Email == registerRequest.Email);

            if (checkUserExist != null)
            {
                throw new ApiException("USERNAME_OR_EMAIL_EXISTS");
            }

            User user = new User
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = HashPassword(registerRequest.Password),
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var response = _mapper.Map<RegisterResponse>(user);
            response.JwtToken = GenarateToken(user);
            return response;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == loginRequest.Email);
            if (user is null)
            {
                throw new ApiException("ACCOUNT_NOT_REGISTERED");
            }

            Console.WriteLine("USER: ", user);

            if (VerifyPassword(loginRequest.Password, user.Password) is false)
            {
                throw new ApiException("PASSWORD_INCORRECT");
            }

            var response = _mapper.Map<LoginResponse>(user);
            response.JwtToken = GenarateToken(user);

            return response;
        }

        public bool ChangePassword(int userId, ChangePassword changePassword)
        {
            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                throw new ApiException("PASSWORD_NOT_MATCH");
            }
            
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new ApiException("USER_NOT_FOUND");
            }

            if (VerifyPassword(changePassword.OldPassword, user.Password) is false)
            {
                throw new ApiException("OLD_PASSWORD_INCORRECT");
            }

            string hassPassword = HashPassword(changePassword.NewPassword);

            user.Password = hassPassword;
            _context.SaveChanges();

            return true;
        }

        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Mã hóa mật khẩu với salt
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string GenarateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfigs.SecretKeyToken));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Role", user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(_appConfigs.ExpiresToken),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
