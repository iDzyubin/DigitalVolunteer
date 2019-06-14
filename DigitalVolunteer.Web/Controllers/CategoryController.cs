using System;
using DigitalVolunteer.Core.DataModels;
using Microsoft.AspNetCore.Mvc;
using DigitalVolunteer.Core.Interfaces;

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
        public IActionResult Add() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add( Category item )
        {
            if (!ModelState.IsValid)
            {
                return View( item );
            }
            _categoryRepository.Add( item );
            return RedirectToMainPage();
        }


        [HttpGet]
        public IActionResult Update( Guid id ) => View( _categoryRepository.Get( id ) );


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update( Category item )
        {
            if (!ModelState.IsValid)
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


        [NonAction]
        private IActionResult RedirectToMainPage() => RedirectToAction( "Index", "Category" );
    }
}