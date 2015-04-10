using System;
using Aggregator.Contracts;
using Aggregator.Dal;
using DalUser = Aggregator.Dal.Domain.User;

namespace Aggregator.Data
{
    public class UserManager :
        IUserManager
    {
        private readonly IDuplexObjectConverter<IUser, DalUser> _userConverter = new DuplexObjectConverter<IUser, User, DalUser>();

        private readonly PasswordHelper _passwordHelper = new PasswordHelper();

        public IUser Register(string login, string password)
        {
            using (var da = new UserDa())
            {
                var existingUser = da.GetFirst(u => u.Login == login);
                if (existingUser != null)
                    return null;

                var salt = _passwordHelper.CreateSalt();
                var user = new User
                {
                    Login = login,
                    PasswordHash = _passwordHelper.GenerateSaltedHash(password, salt),
                    Salt = salt,
                    DateRegistered = DateTime.Now
                };
                var dalUser = _userConverter.Convert(user);
                da.Save(dalUser);
                return _userConverter.Convert(dalUser);
            }
        }

        public IUser CheckAndGet(string login, string password)
        {
            using (var da = new UserDa())
            {
                var existingUser = da.GetFirst(u => u.Login == login);
                if (existingUser == null)
                    return null;

                var testHash = _passwordHelper.GenerateSaltedHash(password, existingUser.Salt);
                if (testHash != existingUser.PasswordHash)
                    return null;

                return _userConverter.Convert(existingUser);
            }
        }
    }
}
