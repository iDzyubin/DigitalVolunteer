using System;
using DigitalVolunteer.API.Filters;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.API.Controllers
{
    [ApiController]
    [Authorize( Roles = "IsAdmin" )]
    [Route( "api/[controller]" )]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;


        public CategoryController( ICategoryRepository categoryRepository )
            => _categoryRepository = categoryRepository;


        [HttpGet( "[action]" )]
        public IActionResult All() => Ok( _categoryRepository.GetAll() );


        [HttpGet( "{id}" )]
        [ServiceFilter( typeof( ValidateCategoryExistsAttribute ) )]
        public IActionResult Get( Guid id ) => Ok( _categoryRepository.Get( id ) );


        [HttpPost( "[action]" )]
        [ModelValidator]
        public IActionResult Create( [FromBody] Category category )
        {
            _categoryRepository.Add( category );
            return Ok();
        }


        [HttpPut( "{id}" )]
        [ModelValidator]
        [ServiceFilter( typeof( ValidateCategoryExistsAttribute ) )]
        public IActionResult Update( Guid id, [FromBody] Category category )
        {
            if( id != category.Id ) return BadRequest( "Id is not valid" );
            _categoryRepository.Update( category );
            return Ok();
        }


        [HttpDelete( "{id}" )]
        [ServiceFilter( typeof( ValidateCategoryExistsAttribute ) )]
        public IActionResult Delete( Guid id )
        {
            _categoryRepository.Remove( id );
            return Ok();
        }
    }
}