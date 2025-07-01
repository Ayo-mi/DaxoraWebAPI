namespace DaxoraWebAPI.Common.DTOs.ApiUser
{
    public class UserPayload
    {
        public Guid UserId { get; set; } = Guid.Empty!;

        public string UserType { get; set; } = null!;

        public string? Email { get; set; }
    }
}
