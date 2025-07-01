using DaxoraWebAPI.Common.DTOs.ApiUser;
using DaxoraWebAPI.Common.Interfaces;
using System.Security.Claims;

namespace DaxoraWebAPI.Authentication
{
    public class UserManager : IUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public UserPayload? GetUserPayload()
        {
            var user = new UserPayload();
            if (_contextAccessor != null && _contextAccessor.HttpContext != null)
            {
                try
                {
                    var payload = _contextAccessor.HttpContext.User;
                    user.Email = payload.FindFirstValue("Email");
                    user.UserId = Guid.Parse(payload.FindFirstValue("UserId"));
                    user.UserType = payload.FindFirstValue("UserType");
                }
                catch (Exception)
                {
                    user = null;
                }
            }
            return user;
        }
    }
}
