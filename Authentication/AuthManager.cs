using DaxoraWebAPI.Common.DTOs.ApiUser;
using DaxoraWebAPI.Common.Interfaces;
using DaxoraWebAPI.Constants;
using DaxoraWebAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DaxoraWebAPI.Authentication
{
    internal class AuthManager(IConfiguration _configuration, DaxoraDbContext _context) : IAuthManager
    {
        private UserClaims? _user;
        public string CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims();
            var token = GenerateToken(claims, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<object?> GetUserDataAsync(byte[] userId)
        {
            try
            {
                var result = await _context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .Where(u => u.UserId == userId && u.Active == 1)
                .Select(x => new
                {
                    UserId = new Guid(x.UserId),
                    x.Email,
                    x.AccountType,
                    Profile = new
                    {
                        x.UserProfile.FirstName,
                        x.UserProfile.LastName,
                    }
                })
                .SingleOrDefaultAsync();

                _user = new UserClaims { Email = result.Email, UserId = result.UserId, UserType = result.AccountType };

                return result;
            }
            catch (Exception _)
            {

                return null;
            }

        }

        private JwtSecurityToken GenerateToken(List<Claim> claims, SigningCredentials signingCredentials)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddYears(Convert.ToInt16(jwtSettings.GetSection("LifeTime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );
            return token;
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>();
            if (_user != null)
            {
                claims = new List<Claim>
                {
                    new Claim("UserType", _user.UserType),
                    new Claim("UserId", _user.UserId.ToString()),
                    new Claim("Email", _user.Email ?? ""),
                };
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Constant.DaxoraKey;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
