using System;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public UserController( UserService userService, TaskService taskService )
        {
            _userService = userService;
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult Index( Guid id )
        {
            var userInfo = _userService.GetExtendedUserInfo( id );
            ViewBag.LastTasks = _taskService.GetUserTaskTitles( id, 3 );
            return View( "Profile", userInfo );
        }

        [HttpGet]
        public IActionResult List()
        {
            if( User.IsAdmin() )
            {
                var users = _userService.GetUsers();
                return View( users );
            }
            else
                return StatusCode( 403 );
        }

        [HttpGet]
        public IActionResult Add()
        {
            if( !User.IsAdmin() )
                return StatusCode( 403 );
            return View();
        }

        [HttpPost]
        public IActionResult Add( RegistrationViewModel model )
        {
            if( !User.IsAdmin() )
                return StatusCode( 403 );
            if( ModelState.IsValid )
            {
                try
                {
                    _userService.Register( new UserRegistrationModel
                    {
                        Email = model.Email.ToLower(),
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone
                    }, model.IsAdmin );
                    return RedirectToAction( nameof( List ) );
                }
                catch( Exception )
                {
                    ViewBag.ErrorMessage = "Произошла ошибка. Свяжитесь с администратором";
                }
            }
            return View( model );
        }

        [HttpGet]
        public IActionResult Edit( Guid id )
        {
            if( !User.IsAdmin() )
                return StatusCode( 403 );
            var user = _userService.GetExtendedUserInfo( id );
            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                IsAdmin = user.IsAdmin
            };
            return View( model );
        }

        [HttpPost]
        public IActionResult Edit( EditUserViewModel model )
        {
            if( !User.IsAdmin() )
                return StatusCode( 403 );
            if( ModelState.IsValid )
            {
                try
                {
                    var user = _userService.GetUser( model.Id );
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Phone = model.Phone;
                    user.IsAdmin = model.IsAdmin;
                    _userService.EditUser( user );
                    return RedirectToAction( nameof( List ) );
                }
                catch( Exception )
                {
                    ViewBag.ErrorMessage = "Произошла ошибка. Свяжитесь с администратором";
                }
            }
            return View( model );
        }

        [HttpGet]
        public IActionResult Delete( Guid id )
        {
            if( !User.IsAdmin() )
                return StatusCode( 403 );
            try
            {
                _userService.Delete( id );
                return RedirectToAction( nameof( List ) );
            }
            catch
            {
                return StatusCode( 400 );
            }
        }
    }
}