using Core.DTOs.GenreDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.GenreValidators
{
    internal class GenreEditValidator : AbstractValidator<GenreEditDTO>
    {
        public GenreEditValidator()
        {
            this.RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
