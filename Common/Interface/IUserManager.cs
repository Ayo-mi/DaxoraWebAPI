using DaxoraWebAPI.Common.DTOs.ApiUser;

namespace DaxoraWebAPI.Common.Interfaces
{
    public interface IUserManager
    {
        UserPayload? GetUserPayload();
    }
}
