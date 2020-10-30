using System;

namespace RailEmu.Auth.Database.Models
{
    public class Account
    {
        public Guid UId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public bool IsAdmin { get; set; }
        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public bool Banned { get; set; }
        public DateTime SuspendedEndDate { get; set; }
        public sbyte Community { get; set; }
        public int LastServer { get; set; }
    }
}
