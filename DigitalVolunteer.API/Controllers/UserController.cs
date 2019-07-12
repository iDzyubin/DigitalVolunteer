using System;
using DigitalVolunteer.API.Extensions;
using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.API.Controllers
{
    /// <summary>
    /// TODO. Add authorization.
    /// TODO. TEST THIS.
    /// </summary>
    [ApiController]
    [Route( "api/[controller]" )]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;


        public UserController( UserService userService, TaskService taskService )
        {
            _userService = userService;
            _taskService = taskService;
        }


        /// <summary>
        /// Get all users.
        /// </summary>
        [HttpGet( "[action]" )]
        [Authorize]
        public IActionResult All() => Ok( _userService.GetUsers() );


        /// <summary>
        /// Get user by id.
        /// </summary>
        [HttpGet( "{id}" )]
        [Authorize]
        [ServiceFilter( typeof( ValidateUserExistsAttribute ) )]
        public IActionResult Get( Guid id ) => Ok( _userService.GetUser( id ) );


        /// <summary>
        /// Get user profile by id.
        /// </summary>
        [HttpGet( "{id}" )]
        [ServiceFilter( typeof( ValidateUserExistsAttribute ) )]
        public IActionResult Profile( Guid? id )
        {
            id = id ?? User.GetId();
            if( id == null ) return Unauthorized( "User is not authorized" );

            var userInfo  = _userService.GetExtendedUserInfo( id.Value );
            var lastTasks = _taskService.GetUserTaskTitles( id.Value, 3 );
            return Ok( new { userInfo, lastTasks } );
        }


        // TODO. TEST
        /// <summary>
        /// Add new user.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ModelValidator]
        public IActionResult Add( [FromBody] RegistrationViewModel model )
        {
            try
            {
                var registrationModel = new UserRegistrationModel
                {
                    Email     = model.Email.ToLower(),
                    Password  = model.Password,
                    FirstName = model.FirstName,
                    LastName  = model.LastName,
                    Phone     = model.Phone
                };
                _userService.Register( registrationModel, model.IsAdmin );
                return Ok();
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        // TODO. TEST
        /// <summary>
        /// Edit user information.
        /// </summary>
        [HttpPut( "{id}" )]
        [Authorize]
        public IActionResult Edit( Guid id, EditUserViewModel model )
        {
            if( IsUserNotFound( id ) ) return NotFound( $"User with id='{id}' not found" );

            try
            {
                _userService.EditUser( model.ConvertToUser() );
                return Ok();
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        // TODO. TEST
        /// <summary>
        /// Delete user.
        /// </summary>
        [HttpDelete]
        [Authorize]
//        [Authorize( Roles = "Admin" )]
        [ServiceFilter( typeof( ValidateUserExistsAttribute ) )]
        public IActionResult Delete( Guid id )
        {
            try
            {
                _userService.Delete( id );
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        [NonAction]
        private bool IsUserNotFound( Guid id ) => _userService.GetUser( id ) == null;
    }
}