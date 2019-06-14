using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;


        public CategoryController( ICategoryRepository categoryRepository )
            => _categoryRepository = categoryRepository;


        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        public IActionResult Create( Category item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }

            _categoryRepository.Add( item );
            return RedirectToMainPage();
        }


        [HttpGet( "{id}" )]
        public IActionResult Update( Guid id ) => View( Get( id ) );


        [HttpPost]
        public IActionResult Update( Category item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }

            _categoryRepository.Update( item );
            return RedirectToMainPage();
        }


        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _categoryRepository.Remove( id );
            return RedirectToMainPage();
        }


        private IActionResult RedirectToMainPage() => RedirectToAction( "Index", "Category" );


        [NonAction]
        private Category Get( Guid id ) => _categoryRepository.Get( id );
    }
}