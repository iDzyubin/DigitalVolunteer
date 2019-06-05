using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Core.DomainModels
{
    /// <summary>
    /// Категория заданий.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        [Display( Name = "Название категории" )]
        public string Name { get; set; }
    }
}