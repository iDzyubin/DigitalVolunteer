using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using FluentValidation;

namespace DigitalVolunteer.Core.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator( ICategoryRepository categoryRepository )
        {
            RuleFor( x => x.Name )
               .NotEmpty()
               .WithMessage( "Наименование категории не заполнено" );

            // Все наименования категорий уникальны.
            RuleFor( x => x )
               .Must( x => categoryRepository
                          .Get( c => c.Name.Trim().ToLower().Equals( x.Name.Trim().ToLower() ) )
                          .FirstOrDefault() == null )
               .WithMessage( "Категория с таким наименованием уже имеется" );
        }
    }
}