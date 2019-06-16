using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class ExecutorController : Controller
    {
        private readonly IUserRepository _userRepository;


        public ExecutorController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet( "{id}" )]
        public IActionResult Details( Guid id ) => View( Get( id ) );


        [HttpGet]
        public IActionResult AcceptTask()
        {
            // Some code here.
            return Ok();
        }


        [NonAction]
        private User Get( Guid id ) => _userRepository.Get( id );
    }
}