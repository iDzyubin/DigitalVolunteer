using System;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using FluentValidation;

namespace DigitalVolunteer.Core.Validators
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator( ITaskRepository taskRepository )
        {
            RuleFor( x => x.Title )
                .NotEmpty()
                .WithMessage( "Наименование задачи не заполнено" )
                .MaximumLength( 50 )
                .WithMessage( "Длина наименования задачи не должно превышать 50 символов" );

            RuleFor( x => x.Description )
                .NotEmpty()
                .WithMessage( "Описание задачи не заполнено" )
                .MaximumLength( 500 )
                .WithMessage( "Длина описания задачи не должно превышать 50 символов" );

            RuleFor( x => x.ContactPhone )
                .NotEmpty()
                .WithMessage( "Контактный телефон не указан" )
                .Matches( @"^((\+7|7|8)+([0-9]){10})$" )
                .WithMessage( "Телефон должен соответствовать следующему формату: 89991234567" );

            RuleFor( x => x.StartDate )
                .NotEmpty()
                .WithMessage( "Дата начала задачи не заполнена" );

            RuleFor( x => x.StartDate )
                .GreaterThanOrEqualTo( DateTime.Now.Date )
                .WithMessage( "Дата начала не может быть ранее чем сегодня" )
                .When( x => taskRepository.Get( x.Id ) == null );

            RuleFor( x => x.EndDate )
                .GreaterThanOrEqualTo( DateTime.Now.Date )
                .WithMessage( "Дата окончания не может быть ранее чем сегодня" )
                .When( x => x.EndDate.HasValue );

            RuleFor( x => x.EndDate )
                .Must( ( x, endDate ) => DateTime.Compare( x.StartDate.Value, endDate.Value ) < 0 )
                .WithMessage( "Начальная дата не может быть позднее конечной" )
                .When( x => x.StartDate.HasValue && x.EndDate.HasValue );
        }
    }
}