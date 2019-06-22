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


        /// <summary>
        /// Просмотреть все задачи.
        /// Возможна сортировка по категориям.
        /// </summary>
        [HttpGet( "[controller]/All" )]
        public IActionResult Index( Guid? categoryId = null )
        {
            var tasks = categoryId.HasValue
                ? _taskRepository.GetAll(categoryId.Value)
                : _taskRepository.GetAll();
            return View( new TaskViewModel { CategoryId = categoryId ?? Guid.Empty, Tasks = tasks } );
        }


        /// <summary>
        /// Информация по задаче.
        /// </summary>
        [HttpGet( "[controller]/{id}" )]
        public IActionResult Details( Guid id ) => View( Get( id ) );


        [Authorize]
        [HttpGet]
        public IActionResult Add() => View( new DigitalTask() );


        /// <summary>
        /// Добавление новой задачи.
        /// </summary>
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


        /// <summary>
        /// Обновление информации по задаче.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update( DigitalTask item )
        {
            if( !ModelState.IsValid ) return View( item );
            _taskRepository.Update( item );
            return RedirectToMainPage();
        }


        /// <summary>
        /// Удаление задачи.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _taskRepository.Remove( id );
            return RedirectToMainPage();
        }


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
        /// Предложить услуги по выполнению задачи.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult OfferHelp( Guid taskId )
        {
            _taskService.OfferHelp( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Принять предложение по выполнению задачи.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult ConfirmOffer( Guid taskId )
        {
            _taskService.ConfirmOffer( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Подтвердить выполнение задачи.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult ConfirmComplete( Guid taskId )
        {
            _taskService.ConfirmComplete( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Завершить задание.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult CloseTask( Guid taskId )
        {
            _taskService.CloseTask( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        /// <summary>
        /// Отменить задание.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult CancelTask( Guid taskId )
        {
            _taskService.CancelTask( taskId, User.GetId().Value );
            return RedirectToDetails( taskId );
        }


        [NonAction]
        private IActionResult RedirectToMainPage() => RedirectToAction( "Index", "Task" );


        [NonAction]
        private IActionResult RedirectToDetails( Guid id )
            => RedirectToAction( "Details", "Task", new { id } );


        [NonAction]
        private DigitalTask Get( Guid id ) => _taskRepository.Get( id );
    }
}