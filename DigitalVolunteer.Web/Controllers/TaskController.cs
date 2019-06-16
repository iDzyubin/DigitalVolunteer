using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;


        public TaskController( ITaskRepository taskRepository )
        {
            _taskRepository = taskRepository;
        }


        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet( "{id}" )]
        public IActionResult Details( Guid id ) => View( Get( id ) );


        // TODO. Only for authorized.
        [HttpGet]
        public IActionResult Create() => View();


        // TODO. Only for authorized.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add( Task item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }
            
            _taskRepository.Add( item, User.GetId().Value );
            return RedirectToMainPage();
        }


        // TODO. Only for authorized.
        [HttpGet]
        public IActionResult Update( Guid id ) => View( Get( id ) );


        // TODO. Only for authorized.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update( Task item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }
            _taskRepository.Update( item );
            return RedirectToMainPage();
        }


        // TODO. Only for authorized.
        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _taskRepository.Remove( id );
            return RedirectToMainPage();
        }


        [NonAction]
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Task" );


        [NonAction]
        private Task Get( Guid id ) => _taskRepository.Get( id );
    }
}