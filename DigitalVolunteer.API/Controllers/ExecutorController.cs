using System;
using System.Collections.Generic;
using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class ExecutorController : ControllerBase
    {
        private readonly IUserRepository _userRepository;


        public ExecutorController( IUserRepository userRepository )
            => _userRepository = userRepository;


        [HttpGet( "[action]/{id}" )]
        [ServiceFilter( typeof( ValidateUserExistsAttribute ) )]
        public IEnumerable<User> All( Guid id )
            => _userRepository.Get( x => x.Id != id );
    }
}