using System.Threading.Tasks;
using DigitalVolunteer.Core.DomainModels;
using DigitalVolunteer.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User>   _userManager;
        private readonly SignInManager<User> _signInManager;

        
        public AccountController( UserManager<User> userManager, SignInManager<User> signInManager )
        {
            _userManager   = userManager;
            _signInManager = signInManager;
        }

        
        [HttpGet]
        public IActionResult Register() => View();

        
        [HttpPost]
        public async Task<IActionResult> Register( RegisterViewModel model )
        {
            if( !ModelState.IsValid ) return View( model );

            var user   = new User { Email = model.Email, UserName = model.Email, Login = model.Login };
            var result = await _userManager.CreateAsync( user, model.Password );

            if( !result.Succeeded )
            {
                foreach( var error in result.Errors )
                {
                    ModelState.AddModelError( string.Empty, error.Description );
                }

                return View( model );
            }

            await _signInManager.SignInAsync( user, false );
            return RedirectToAction( "Index", "Home" );
        }
    }
}