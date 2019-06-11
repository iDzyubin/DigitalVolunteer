using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController( UserService userService )
        {
            _userService = userService;
        }

        private async Task Authenticate( string email )
        {
            var claims = new List<Claim> { new Claim( ClaimsIdentity.DefaultNameClaimType, email ) };
            var identity = new ClaimsIdentity( claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType );
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
                    _userService.Register( new UserRegistrationModel { Email = model.Email.ToLower(), Password = model.Password } );
                    await Authenticate( model.Email.ToLower() );
                    return RedirectToAction( "Index", "Home" );
                }
                catch( Exception ex )
                {
                    model.ErrorMessage = ex.Message;
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
    }
}