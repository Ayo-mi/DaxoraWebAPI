using System;
using System.Collections.Generic;

namespace DaxoraWebAPI.Entities;

public partial class User
{
    public byte[] UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte Active { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
