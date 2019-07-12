using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.Models;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    public class AccountController : Controller
    {
        private readonly UserService _userService;


        public AccountController( UserService userService )
        {
            _userService = userService;
        }


        // TODO. Validation is not working.
        /// <summary>
        /// User registration.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        [ServiceFilter( typeof( UserRegisteredValidatorAttribute ) )]
        public IActionResult Registration( [FromBody] RegistrationViewModel model )
        {
            try
            {
                _userService.Register( new UserRegistrationModel
                {
                    Email     = model.Email.ToLower(),
                    Password  = model.Password,
                    FirstName = model.FirstName,
                    LastName  = model.LastName,
                    Phone     = model.Phone
                } );
                return Ok
                (
                    "На указанный e-mail было отправленно письмо. " +
                    "Для завершения регистрации перейдите по ссылке в нем."
                );
            }
            catch( Exception )
            {
                return BadRequest( "Произошла ошибка. Свяжитесь с администратором" );
            }
        }


        /// <summary>
        /// Login user into system.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public async Task<IActionResult> Login( LoginViewModel model )
        {
            var validationResult = _userService.Validate( model.Email, model.Password );

            switch( validationResult )
            {
                case UserService.ValidationResult.Success:
                    await Authenticate( model.Email );
                    return Ok( "Авторизация прошла успешно" );
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

            return BadRequest( ModelState );
        }


        /// <summary>
        /// Logout account.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme );
            return Ok();
        }


        /// <summary>
        /// Confirm account by link via code.
        /// </summary>
        [HttpGet]
        public IActionResult Confirm( string code )
        {
            try
            {
                _userService.ConfirmAccount( code );
                return Ok( "Регистрация завершена. Теперь Вы можете войти под своим аккаунтом" );
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        private async Task Authenticate( string email )
        {
            var user = _userService.GetUserInfo( email );
            var claims = new List<Claim>
            {
                new Claim( ClaimTypes.Name,      user.Email ),
                new Claim( DvClaimTypes.UserId,  user.Id.ToString() ),
                new Claim( DvClaimTypes.IsAdmin, user.IsAdmin.ToString() )
            };
            var identity = new ClaimsIdentity( claims, "email" );
            await HttpContext.SignInAsync
            (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal( identity )
            );
        }
    }
}