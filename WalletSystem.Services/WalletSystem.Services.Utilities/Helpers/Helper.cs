using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WalletSystem.Services.Utilities.Helpers
{
    public class Helper
    {
        private readonly IConfiguration _configuration;


        public Helper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string id, string role)
        {
            var mySecret = _configuration["Jwt:SigningKey"];
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userID", id),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //public Response VerifyToken(HttpContext context)
        //{
        //    try
        //    {
        //        var secret = _configuration.GetSection("JWT:SECRET").Value;
        //        var token = context.Request.Headers["Authorization"].ToString().Split(" ")[1];
        //        var handler = new JwtSecurityTokenHandler();
        //        var decoded = handler.ReadJwtToken(token);

        //        if (decoded == null) return new Response
        //        { Message = "The token provided is invalid" };

        //        var parameters = new TokenValidationParameters()
        //        {
        //            RequireExpirationTime = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
        //        };
        //        var principal = handler.ValidateToken(token, parameters, out var securityToken).Claims.First();

        //        var user = _userManager.Users.FirstOrDefault(x => x.Id == principal.Value);

        //        return user == null ? new Response { Message = "The token provided is invalid" } : new Response { UserId = user.Id, Flagged = true };
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //        return new Response { Message = "Token not provided" };
        //    }
        //    catch (Exception e)
        //    {
        //        return new Response { Message = e.Message };
        //    }
        //}
    }
}

