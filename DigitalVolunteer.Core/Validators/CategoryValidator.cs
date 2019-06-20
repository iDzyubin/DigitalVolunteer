using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using FluentValidation;

namespace DigitalVolunteer.Core.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryValidator( ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor( x => x.Name )
                .NotEmpty()
                .WithMessage( "Название категории не заполнено" )

                .MaximumLength( 50 )
                .WithMessage( "Название категории не может быть более 50 символов" )

                .Must( IsNameUnique )
                .WithMessage( "Категория с таким названием уже имеется" );
        }

        private bool IsNameUnique( Category category, string newValue )
            => _categoryRepository
                    .Get( c => c.Name.Trim().ToLower().Equals( newValue.Trim().ToLower() ) )
                    .FirstOrDefault() == null;
    }
}