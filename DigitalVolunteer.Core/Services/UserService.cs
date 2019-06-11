using System;
using System.Collections.Generic;
using System.Text;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DigitalVolunteer.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _users;
        private readonly PasswordHashService _hashService;
        private readonly ILogger _logger;

        public UserService( IUserRepository userRepository, PasswordHashService hashService, ILogger<UserService> logger )
        {
            _users = userRepository;
            _hashService = hashService;
            _logger = logger;
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
                _logger.LogError( ex, "Registration failed" );
                throw new Exception( "Произошла непредвиденная ошибка" );
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
    }
}
