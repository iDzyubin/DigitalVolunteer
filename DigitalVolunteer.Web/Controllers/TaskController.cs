using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Web.Models;
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


        [HttpGet( "[controller]/All" )]
        public IActionResult Index() => View();


        [HttpGet( "[controller]/{id}" )]
        public IActionResult Details( Guid id ) => View( Get( id ) );


        [HttpGet]
        public IActionResult Add() => View( new DigitalTask() );


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add( DigitalTask item )
        {
            if( !User.GetId().HasValue ) return RedirectToAction( "Login", "Account" );

            if( !ModelState.IsValid )
            {
                return View( item );
            }

            _taskRepository.Add( item, User.GetId().Value );
            return RedirectToMainPage();
        }


        [HttpGet]
        public IActionResult Update( Guid id ) => View( Get( id ) );


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update( DigitalTask item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }
            _taskRepository.Update( item );
            return RedirectToMainPage();
        }


        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _taskRepository.Remove( id );
            return RedirectToMainPage();
        }


        [HttpGet]
        public IActionResult MyTasks( TaskSelectorMode selectorMode )
        {
            if( !User.GetId().HasValue ) return RedirectToAction( "Login", "Account" );

            return View( new TaskViewModel
            {
                SelectorMode = selectorMode,
                Tasks = _taskRepository.GetMyTasks( User.GetId().Value, selectorMode )
            } );
        }


        public IActionResult OfferTaskHelp( Guid taskId )
        {
            // TODO. Предложить свои услуги по выполнению конкретной задачи.
            return Ok();
        }


        [NonAction]
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Task" );


        [NonAction]
        private DigitalTask Get( Guid id ) => _taskRepository.Get( id );
    }
}