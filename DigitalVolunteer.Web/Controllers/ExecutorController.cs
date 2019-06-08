using System;
using DigitalVolunteer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    [Route( "[controller]" )]
    public class ExecutorController : Controller
    {
        private readonly ExecutorService _executorService;

        
        public ExecutorController( ExecutorService executorService )
            => _executorService = executorService;

        
        [HttpGet]
        [Route( "List" )]
        public IActionResult Index()
            => View();

        
        [HttpGet( "{id}" )]
        public IActionResult Card( Guid id )
            => View( _executorService.GetInformationAboutExecutor( id ) );
    }
}