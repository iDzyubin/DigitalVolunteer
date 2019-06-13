using System;
using System.Collections.Generic;
using System.Text;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;

namespace DigitalVolunteer.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _users;
        private readonly PasswordHashService _hashService;

        public UserService( IUserRepository userRepository, PasswordHashService hashService )
        {
            _users = userRepository;
            _hashService = hashService;
        }

        public void Register( UserRegistrationModel model )
        {
            var isEmailTaken = _users.Get( u => u.Email == model.Email ) == null;
            if( isEmailTaken )
            {
                throw new Exception( "Аккаунт с таким e-mail уже зарегистрирован" );
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Password = _hashService.GetHash( model.Password ),
                Status = UserStatus.Unconfirmed,
                RegistrationDate = DateTime.Now
            };
            try
            {
                _users.Add( user );
            }
            catch( Exception ex )
            {
                throw new Exception( "Произошла непредвиденная ошибка", ex );
            }
        }

        public enum ValidationResult
        {
            Success,
            UserNotExist,
            PasswordNotMatch
        }

        public ValidationResult Validate( string email, string password )
        {
            var user = _users.GetByEmail( email );
            return user == null
                ? ValidationResult.UserNotExist
                : _hashService.ValidateHash( password, user.Password )
                    ? ValidationResult.Success
                    : ValidationResult.PasswordNotMatch;
        }

        public UserInfoModel GetUserInfo( string email )
        {
            var user = _users.GetByEmail( email );
            return user == null ? null : new UserInfoModel
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }
    }
}
