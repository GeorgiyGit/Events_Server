using Core.DTOs.GenreDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.GenreValidators
{
    internal class GenreCreateValidator : AbstractValidator<GenreCreateDTO>
    {
        public GenreCreateValidator()
        {
            this.RuleFor(e => e.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(StaticProperties.GENRE_MIN_LENGTH)
                .MaximumLength(StaticProperties.GENRE_MAX_LENGTH);
        }
    }
}
