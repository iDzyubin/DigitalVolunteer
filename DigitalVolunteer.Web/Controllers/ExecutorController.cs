﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class ExecutorController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet]
        public IActionResult OfferTask( Guid taskId, Guid executorId )
        {
            // TODO. Предлагаем исполнителю выполнить одно из наших заданий.
            return Ok();
        }
    }
}