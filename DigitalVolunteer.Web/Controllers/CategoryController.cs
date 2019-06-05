using System;
using DigitalVolunteer.Core.DomainModels;
using DigitalVolunteer.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    /// <summary>
    /// Контроллер для работы с категориями.
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        /// <summary>
        /// Basic ctor.
        /// </summary>
        /// <param name="categoryRepository"></param>
        public CategoryController( IRepository<Category> categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }


        /// <summary>
        /// Страница с категориями.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index() => View();


        /// <summary>
        /// Страница для добавления категории.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add() => View();


        /// <summary>
        /// Добавляем категорию.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add( Category item )
        {
            if( !ModelState.IsValid )
            {
                return View( item );
            }

            _categoryRepository.Add( item );
            return RedirectToMainPage();
        }


        /// <summary>
        /// Страница обновления категории.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Update( Guid id )
        {
            var model = _categoryRepository.Get( id );
            return View( model );
        }


        /// <summary>
        /// Обновляем категорию.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Удаляем категорию.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Remove( Guid id )
        {
            _categoryRepository.Remove( id );
            return RedirectToMainPage();
        }


        /// <summary>
        /// Перенаправляем на главную.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private IActionResult RedirectToMainPage()
            => RedirectToAction( "Index", "Category" );
    }
}