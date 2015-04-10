using System;

namespace Aggregator.Contracts
{
    public interface IUser
    {
        int Id { get; }

        string Login { get; }

        string PasswordHash { get; }

        string Salt { get; }

        DateTime? DateRegistered { get; }
    }
}