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
<<<<<<< Updated upstream
        public IActionResult Index() => View();
=======
        public IActionResult Index( Guid? categoryId = null )
        {
            var tasks = _taskRepository.GetAll( categoryId );
            return View( new TaskViewModel { CategoryId = categoryId ?? Guid.Empty, Tasks = tasks } );
        }
>>>>>>> Stashed changes


        [HttpGet( "[controller]/{id}" )]
        public IActionResult Details( Guid id ) => View( _taskRepository.GetTaskDetails( id ) );


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
        public IActionResult Update( Guid id ) => View( _taskRepository.Get( id ) );


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


<<<<<<< Updated upstream
=======
        /// <summary>
        /// Список задач пользователя.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult MyTasks( TaskSelectorMode selectorMode, Guid? categoryId = null ) => View( new TaskViewModel
        {
            SelectorMode = selectorMode,
            Tasks = _taskRepository.GetMyTasks( User.GetId().Value, selectorMode, categoryId ),
            CategoryId = categoryId ?? Guid.Empty
        } );


        /// <summary>
        /// Предложить заказчику услуги по выполнению задачи.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult OfferHelp( Guid taskId )
        {
            _taskService.OfferHelp( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Предложить задачу потенциальному исполнителю.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult OfferTask( Guid taskId, Guid executorId )
        {
            _taskService.OfferTask( taskId, executorId );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Принять предложение по выполнению задачи.
        /// </summary>
        [Authorize]
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Task" );


        [NonAction]
        private DigitalTask Get( Guid id ) => _taskRepository.Get( id );
=======
        private IActionResult RedirectToDetails( Guid id )
            => RedirectToAction( "Details", "Task", new { id } );
>>>>>>> Stashed changes
    }
}