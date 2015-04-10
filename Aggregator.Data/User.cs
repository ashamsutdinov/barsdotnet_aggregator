using System;
using Aggregator.Contracts;

namespace Aggregator.Data
{
    public class User : 
        IUser
    {
        public int Id { get; internal set; }

        public string Login { get; internal set; }

        public string PasswordHash { get; internal set; }

        public string Salt { get; internal set; }

        public DateTime? DateRegistered { get; internal set; }
    }
}