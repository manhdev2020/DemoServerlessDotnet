using DemoServerless.Configs;
using DemoServerless.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DemoServerless.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppConfigs _appConfigs;

        public JwtMiddleware(RequestDelegate next, IOptions<AppConfigs> appConfigs)
        {
            _next = next;
            _appConfigs = appConfigs.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tokenString = GetTokenFromHeader(context);

            if (tokenString != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfigs.SecretKeyToken));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var principal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
                    int userId = int.Parse(principal.Claims.First(x => x.Type == "UserId").Value);
                    Role role = (Role)Enum.Parse(typeof(Role), principal.Claims.First(x => x.Type == "Role").Value);

                    context.Items["User"] = new User
                    {
                        Id = userId,
                        Role = role,
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token is invalid: {ex.Message}");
                }
            }

            await _next(context);
        }

        private string GetTokenFromHeader(HttpContext context)
        {
            // Extract the token from the header
            // You may need to customize this code to fit your authentication scheme
            var authorizationHeader = context.Request.Headers["Authorization"];
            if (authorizationHeader.Count > 0)
            {
                var tokenType = authorizationHeader[0].Split(' ')[0];
                if (tokenType.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return authorizationHeader[0].Split(' ')[1];
                }
            }

            return null;
        }
    }

}
