using DigitalVolunteer.Core.DomainModels;
using FluentValidation;

namespace DigitalVolunteer.Core.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor( x => x.Name )
               .NotEmpty()
               .WithMessage( "Необходимо заполнить название категории" )
               .MaximumLength( 50 )
               .WithMessage( "Длина названия не может превышать 50 символов" );
        }
    }
}