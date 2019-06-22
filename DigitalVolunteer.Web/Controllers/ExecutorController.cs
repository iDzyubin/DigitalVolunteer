using System;
using DigitalVolunteer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class ExecutorController : Controller
    {
        private readonly TaskService _taskService;

        public ExecutorController( TaskService taskService )
            => _taskService = taskService;


        [HttpGet]
        public IActionResult Index() => View();


        [HttpPost]
        public IActionResult OfferTask( Guid taskId, Guid executorId )
        {
            _taskService.OfferHelp( taskId, executorId );
            return RedirectToAction( "Index" );
        }
    }
}