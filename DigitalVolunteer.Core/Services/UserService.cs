using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;

namespace DigitalVolunteer.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _users;
        private readonly ICategoryRepository _categories;
        private readonly TaskService _taskService;
        private readonly PasswordHashService _hashService;

        public UserService( IUserRepository userRepository, ICategoryRepository categoryRepository,
            TaskService taskService, PasswordHashService hashService )
        {
            _users = userRepository;
            _categories = categoryRepository;
            _taskService = taskService;
            _hashService = hashService;
        }

        public void Register( UserRegistrationModel model, bool isAdmin = false )
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
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                IsAdmin = isAdmin,
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

        public void Delete( Guid id )
        {
            _users.Remove( id );
        }

        public object GetUsers()
        {
            return _users.GetAll()
                .Select( u => new UserInfoModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Description = u.Description,
                    Phone = u.Phone,
                    IsAdmin = u.IsAdmin
                } ).ToList();
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

        public User GetUser( Guid id )
        {
            return _users.Get( id );
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

        public void EditUser( User user )
        {
            _users.Update( user );
        }

        public UserInfoModel GetExtendedUserInfo( Guid id )
        {
            var user = _users.Get( id );
            if( user == null )
                throw new KeyNotFoundException( $"Пользователя с ID { id } нет в базе" );

            var createdCount = _taskService.GetCreatedTasksCount( id );
            var completedTasks = _taskService.GetCompletedTasks( id );
            var favoriteCategoryId = completedTasks
                .GroupBy( t => t.CategoryId )
                .Select( g => new { Id = g.Key, Count = g.Count() } )
                .OrderByDescending( g => g.Count )
                .FirstOrDefault()?.Id;
            var favoriteCategory = favoriteCategoryId == null
                ? "Нет"
                : _categories.Get( favoriteCategoryId.Value ).Name;

            return new UserInfoModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Description = user.Description,
                IsAdmin = user.IsAdmin,
                TasksCreated = createdCount,
                TasksCompleted = completedTasks.Count,
                FavoriteCategory = favoriteCategory
            };
        }
    }
}
