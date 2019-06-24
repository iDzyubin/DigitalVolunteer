using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public AccountController( UserService userService, TaskService taskService )
        {
            _userService = userService;
            _taskService = taskService;
        }

        private async Task Authenticate( string email )
        {
            var user = _userService.GetUserInfo( email );
            var claims = new List<Claim>
            {
                new Claim( ClaimTypes.Name, user.Email ),
                new Claim( DvClaimTypes.UserId, user.Id.ToString() ),
                new Claim( DvClaimTypes.IsAdmin, user.IsAdmin.ToString() )
            };
            var identity = new ClaimsIdentity( claims, "email" );
            await HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal( identity ) );
        }

        [HttpGet]
        public IActionResult Registration() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration( RegistrationViewModel model )
        {
            if( !ModelState.IsValid ) return View( model );

            if( _userService.IsUserRegistered( model.Email ) )
            {
                ModelState.AddModelError( "email", "Аккаунт с таким e-mail адресом уже существует" );
                return View( model );
            }

            try
            {
                _userService.Register( new UserRegistrationModel
                {
                    Email = model.Email.ToLower(),
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone
                } );

                ViewBag.AlertType = "alert-info";
                ViewBag.AlertMessage =
                    "На указанный e-mail было отправленно письмо. Для завершения регистрации перейдите по ссылке в нем";
            }
            catch( Exception e )
            {
                ViewBag.AlertType = "alert-danger";
                ViewBag.AlertMessage = "Произошла ошибка. Свяжитесь с администратором";
            }

            return View( "RegistrationResult" );
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model )
        {
            if( ModelState.IsValid )
            {
                var check = _userService.Validate( model.Email.ToLower(), model.Password );
                switch( check )
                {
                    case UserService.ValidationResult.Success:
                        await Authenticate( model.Email.ToLower() );
                        return RedirectToAction( "Index", "Home" );
                    case UserService.ValidationResult.UserNotExist:
                        ModelState.AddModelError( "Email", "Аккаунт с таким e-mail не существует" );
                        break;
                    case UserService.ValidationResult.PasswordNotMatch:
                        ModelState.AddModelError( "Password", "Пароль введен неверно" );
                        break;
                    case UserService.ValidationResult.UserNotConfirmed:
                        ModelState.AddModelError( "Email", "Аккаунт не подтвержден" );
                        break;
                }
            }

            return View( model );
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme );
            return RedirectToAction( "Index", "Home" );
        }

        [HttpGet]
        public IActionResult Profile( Guid? id )
        {
            id = id ?? User.GetId();
            if( id == null ) return RedirectToAction( "Login" );

            var userInfo = _userService.GetExtendedUserInfo( id.Value );
            ViewBag.LastTasks = _taskService.GetUserTaskTitles( id.Value, 3 );
            return View( userInfo );
        }

        [HttpGet]
        public IActionResult Confirm( string code )
        {
            try
            {
                _userService.ConfirmAccount( code );
                ViewBag.AlertMessage = "Регистрация завершена. Теперь Вы можете войти под своим аккаунтом";
                ViewBag.AlertType = "alert-success";
            }
            catch( Exception e )
            {
                ViewBag.AlertMessage = e.Message;
            }

            return View( nameof( Login ) );
        }
    }
}