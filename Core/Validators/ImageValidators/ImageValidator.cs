using Core.DTOs.ImageDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.ImageValidators
{
    internal class ImageValidator : AbstractValidator<ImageDTO>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Path)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Path));

            RuleFor(e => e.Title)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StaticProperties.TITLE_MAX_LENGTH);
        }
    }
}
