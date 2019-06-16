using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DigitalVolunteer.Core.Interfaces;
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
        private readonly IUserRepository _userRepository;

        public AccountController( UserService userService, IUserRepository userRepository )
        {
            _userService = userService;
            _userRepository = userRepository;
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
            await HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal( identity ) );
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration( RegistrationViewModel model )
        {
            if( ModelState.IsValid )
            {
                try
                {
                    _userService.Register( new UserRegistrationModel
                    {
                        Email = model.Email.ToLower(),
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    } );
                    await Authenticate( model.Email.ToLower() );
                    return RedirectToAction( "Index", "Home" );
                }
                catch( Exception )
                {
                    ViewBag.ErrorMessage = "Произошла ошибка. Свяжитесь с администратором";
                }
            }
            return View( model );
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
        [Route("[controller]/Profile/{id}")]
        public IActionResult UserCard( Guid id ) => View( _userRepository.Get( id ) );
    }
}