using System;
using System.Collections.Generic;
using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskService     _taskService;


        public TaskController( ITaskRepository taskRepository, TaskService taskService )
        {
            _taskRepository = taskRepository;
            _taskService    = taskService;
        }


        /// <summary>
        /// All tasks.
        /// Sorting by category is possible.
        /// </summary>
        [HttpGet( "[action]/{id?}" )]
        public IEnumerable<DigitalTask> All( Guid? categoryId = null )
            => _taskRepository.GetAll( categoryId );


//        /// <summary>
//        /// Digital task's details.
//        /// </summary>
//        [HttpGet( "{id}" )]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        public DigitalTask Details( Guid id )
//            => _taskRepository.GetTaskDetails( id );
//

        /// <summary>
        /// Add a new task.
        /// </summary>
        [HttpPost]
        [ModelValidator]
        [ValidateAntiForgeryToken]
        public IActionResult Add( [FromBody] DigitalTask item )
        {
            _taskRepository.Add( item, User.GetId().Value );
            return Ok();
        }


//        /// <summary>
//        /// Update task info.
//        /// </summary>
//        [HttpPut( "{id}" )]
//        [ModelValidator]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        [ValidateAntiForgeryToken]
//        public IActionResult Update( Guid id, [FromBody] DigitalTask item )
//        {
//            _taskRepository.Update( item );
//            return Ok();
//        }
//
//
//        /// <summary>
//        /// Delete task.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Authorize]
//        [HttpDelete( "{id}" )]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        public IActionResult Remove( Guid id )
//        {
//            _taskRepository.Remove( id );
//            return Ok();
//        }


        // TODO.
        /// <summary>
        /// Список задач пользователя.
        /// </summary>
//        [Authorize]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        [HttpGet]
//        public IActionResult MyTasks( TaskSelectorMode selectorMode, Guid? categoryId = null ) => View(
//            new TaskViewModel
//            {
//                SelectorMode = selectorMode,
//                Tasks        = _taskRepository.GetMyTasks( User.GetId().Value, selectorMode, categoryId ),
//                CategoryId   = categoryId ?? Guid.Empty
//            } );


        /// <summary>
        /// Offer help to task owner.
        /// </summary>
//        [Authorize]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        [HttpGet]
//        public IActionResult OfferHelp( Guid taskId )
//        {
//            _taskService.OfferHelp( taskId, User.GetId().Value );
//            return Ok();
//        }
//
//
//        /// <summary>
//        /// Offer task to potential executor.
//        /// </summary>
//        [Authorize]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        [HttpGet]
//        public IActionResult OfferTask( Guid taskId, Guid executorId )
//        {
//            _taskService.OfferTask( taskId, executorId );
//            return Ok();
//        }
//
//
//        /// <summary>
//        /// Confirm offer on task execution.
//        /// </summary>
//        [Authorize]
//        [ServiceFilter( typeof( ValidateEntityExistsAttribute ) )]
//        [HttpGet]
//        public IActionResult ConfirmOffer( Guid taskId )
//        {
//            _taskService.ConfirmOffer( taskId, User.GetId().Value );
//            return Ok();
//        }


        /// <summary>
        /// Confirm that task completed.
        /// </summary>
        [Authorize]
//        [ValidateEntityExists]
        [HttpGet]
        public IActionResult ConfirmComplete( Guid taskId )
        {
            _taskService.ConfirmComplete( taskId, User.GetId().Value );
            return Ok();
        }


        /// <summary>
        /// Close the task card.
        /// </summary>
        [Authorize]
//        [ValidateEntityExists]
        [HttpGet]
        public IActionResult CloseTask( Guid taskId )
        {
            _taskService.CloseTask( taskId, User.GetId().Value );
            return Ok();
        }


        /// <summary>
        /// Cancel the task.
        /// </summary>
        [Authorize]
//        [ValidateEntityExists]
        [HttpGet]
        public IActionResult CancelTask( Guid taskId )
        {
            _taskService.CancelTask( taskId, User.GetId().Value );
            return Ok();
        }
    }
}