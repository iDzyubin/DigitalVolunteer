﻿using System;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class ExecutorController : Controller
    {
        private readonly IExecutorRepository _executorRepository;


        public ExecutorController( IExecutorRepository executorRepository )
        {
            _executorRepository = executorRepository;
        }


        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet( "{id}" )]
        public IActionResult Details( Guid id )
            => View( _executorRepository.Get( id ) );


        [HttpGet]
        public IActionResult OfferTask( Guid taskId, Guid executorId )
        {
            // Предлагаем исполнителю выполнить одно из наших заданий.
            return Ok();
        }
    }
}