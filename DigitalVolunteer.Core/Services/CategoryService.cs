using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DigitalVolunteer.Core.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;


        public CategoryService( ICategoryRepository categoryRepository )
            => _categoryRepository = categoryRepository;


        public SelectList GetCategoriesList()
            => new SelectList( _categoryRepository.GetAll(), "Id", "Name" );
    }
}
