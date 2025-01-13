using System;

namespace FinTrackPro.Models
{
    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public int Balance { get; set; } = 0;
        public Guid UserID { get; set; }
        
    }

}
