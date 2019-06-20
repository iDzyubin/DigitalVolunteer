using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskService _taskService;


        public TaskController( ITaskRepository taskRepository, TaskService taskService )
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
        }


        [HttpGet( "[controller]/All" )]
        public IActionResult Index() => View();


        [HttpGet( "[controller]/{id}" )]
        public IActionResult Details( Guid id ) => View( Get( id ) );


        [Authorize]
        [HttpGet]
        public IActionResult Add() => View( new DigitalTask() );


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add( DigitalTask item )
        {
            if( !ModelState.IsValid ) return View( item );
            _taskRepository.Add( item, User.GetId().Value );
            return RedirectToMainPage();
        }


        [Authorize]
        [HttpGet]
        public IActionResult Update( Guid id ) => View( Get( id ) );


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update( DigitalTask item )
        {
            if( !ModelState.IsValid ) return View( item );
            _taskRepository.Update( item );
            return RedirectToMainPage();
        }


        [Authorize]
        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _taskRepository.Remove( id );
            return RedirectToMainPage();
        }


        [Authorize]
        [HttpGet]
        public IActionResult MyTasks( TaskSelectorMode selectorMode ) => View( new TaskViewModel
        {
            SelectorMode = selectorMode,
            Tasks = _taskRepository.GetMyTasks( User.GetId().Value, selectorMode )
        } );


        [Authorize]
        [HttpGet]
        public IActionResult OfferTaskHelp( Guid taskId )
        {
            _taskService.OfferTask( User.GetId().Value, taskId );
            return RedirectToDetails( taskId );
        }


        [Authorize]
        [HttpGet]
        public IActionResult CancelTaskHelp( Guid taskId )
        {
            _taskService.CancelOffer( User.GetId().Value, taskId );
            return RedirectToDetails( taskId );
        }


        [NonAction]
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Task" );


        [NonAction]
        private IActionResult RedirectToDetails( Guid id )
            => RedirectToAction( "Details", "Task", new { id } );


        [NonAction]
        private DigitalTask Get( Guid id ) => _taskRepository.Get( id );
    }
}