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
        private readonly NotificationService _notificationService;

        public UserService( IUserRepository userRepository, ICategoryRepository categoryRepository,
            TaskService taskService, PasswordHashService hashService )


        public UserService
        (
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            TaskService taskService,
            PasswordHashService hashService,
            NotificationService notificationService
        )
        {
            _users = userRepository;
            _categories = categoryRepository;
            _taskService = taskService;
            _hashService = hashService;
            _notificationService = notificationService;
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
                RegistrationDate = DateTime.Now,
                ConfirmCode = Guid.NewGuid().ToString( "N" )
            };

            try
            {
                _users.Add( user );
                var url = $"/Account/Confirm?code={user.ConfirmCode}";
                _notificationService.SendAccountConfirmation( user.Email, url );
            }
            catch( Exception ex )
            {
                throw new Exception( "Произошла непредвиденная ошибка", ex );
            }
        }


        public void Delete( Guid id ) => _users.Remove( id );


        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id"></param>
        public void Delete( Guid id ) => _users.Remove( id );


        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser( Guid id ) => _users.Get( id );


        /// <summary>
        /// Отредактировать пользователя.
        /// </summary>
        /// <param name="user"></param>
        public void EditUser( User user ) => _users.Update( user );


        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns></returns>
        public object GetUsers()
            => _users.GetAll().Select( u => new UserInfoModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Description = u.Description,
                Phone = u.Phone,
                IsAdmin = u.IsAdmin
            } ).ToList();


        /// <summary>
        /// Результат валидации.
        /// </summary>
        public enum ValidationResult
        {
            Success,
            UserNotExist,
            UserNotConfirmed,
            PasswordNotMatch
        }


        public ValidationResult Validate( string email, string password )
        {
            var user = _users.GetByEmail( email );
            return ( user == null || user.Status == UserStatus.Deleted )
        }

        
        /// <summary>
        /// Проверяем введенные данные при попытке залогиниться.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ValidationResult Validate( string email, string password )
        {
            var user = _users.GetByEmail( email );
            return user == null
                ? ValidationResult.UserNotExist
                : user.Status == UserStatus.Unconfirmed
                    ? ValidationResult.UserNotConfirmed
                    : _hashService.ValidateHash( password, user.Password )
                        ? ValidationResult.Success
                        : ValidationResult.PasswordNotMatch;
        }


        public User GetUser( Guid id ) => _users.Get( id );


        /// <summary>
        /// Получить информацию о пользователе.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserInfoModel GetUserInfo( string email )
        {
            var user = _users.GetByEmail( email );
            return user == null
                ? null
                : new UserInfoModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin
                };
        }


        public void EditUser( User user ) => _users.Update( user );


        /// <summary>
        /// Получить расширенную информацию.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfoModel GetExtendedUserInfo( Guid id )
        {
            var user = _users.Get( id );
            if( user == null )
                throw new KeyNotFoundException( $"Пользователя с ID {id} нет в базе" );

            var createdCount       = _taskService.GetCreatedTasksCount( id );
            var completedTasks     = _taskService.GetCompletedTasks( id );
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


        public void ConfirmAccount( string code )
        {
            var user = _users.Get( u => u.ConfirmCode == code ).FirstOrDefault() ??
                       throw new Exception( "Ссылка недействительна" );
            switch( user.Status )
            {
                case UserStatus.Deleted:
                    throw new Exception( "Аккаунт был удален" );
                case UserStatus.Confirmed:
                    throw new Exception( "Аккаунт был подтвержден ранее" );
                case UserStatus.Unconfirmed:
                case UserStatus.Unknown:
                    user.Status = UserStatus.Confirmed;
                    _users.Update( user );
                    break;
            }
        }


        public bool IsUserRegistered( string email )
            => _users.GetByEmail( email.ToLower() ) != null;
    }
}