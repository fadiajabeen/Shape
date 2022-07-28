using System;
using System.Collections.Generic;

namespace Shape.Models
{
    public partial class UserLoginHistory
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? LoginToken { get; set; }
        public bool IsLoggedIn { get; set; }
        public string? Ip { get; set; }
        public DateTime LoginDate { get; set; }
        public decimal LocalTimeHoursOffset { get; set; }
        public DateTime? LogoutDate { get; set; }
        public DateTime? TokenExpiryDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
