using System;
using DigitalVolunteer.Core.DomainModels;
using DigitalVolunteer.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        
        public TaskController( ITaskRepository taskRepository )
            => _taskRepository = taskRepository;

        
        /// <summary>
        /// Страница со списком задач.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index() => View();


        /// <summary>
        /// Карточка задачи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Card( Guid id ) => View( GetTask( id ) );


        /// <summary>
        /// Страница для добавления новой задачи.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create() => View();


        /// <summary>
        /// Добавить новую задачу.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create( Task task )
        {
            if( !ModelState.IsValid )
            {
                return View( task );
            }

            _taskRepository.Add( task );
            return RedirectToMainPage();
        }

        
        /// <summary>
        /// Страница для обновления информации о задаче.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Update( Guid id ) => View( GetTask( id ) );

        
        /// <summary>
        /// Обновить информацию о задаче.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update( Task task )
        {
            if( !ModelState.IsValid )
            {
                return View( task );
            }

            _taskRepository.Update( task );
            return RedirectToMainPage();
        }

        
        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Delete( Guid id )
        {
            _taskRepository.Remove( id );
            return RedirectToMainPage();
        }

        
        /// <summary>
        /// Вернуть задачу по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NonAction]
        private Task GetTask( Guid id ) => _taskRepository.Get( id );

        
        /// <summary>
        /// Отправить на главную.
        /// </summary>
        /// <returns></returns>
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Executor" );
    }
}