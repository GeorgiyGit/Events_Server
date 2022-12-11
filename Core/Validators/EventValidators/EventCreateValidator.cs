using Core.DTOs.EventDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.EventValidators
{
    public class EventCreateValidator : AbstractValidator<EventCreateDTO>
    {
        public EventCreateValidator()
        {
            RuleFor(e => e.Title)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StaticProperties.TITLE_MAX_LENGTH);

            RuleFor(e => e.Text)
                .NotNull()
                .NotEmpty()
                .MinimumLength(StaticProperties.DESCRIPTION_TEXT_MIN_LENGTH)
                .MaximumLength(StaticProperties.DESCRIPTION_TEXT_MAX_LENGTH);

            RuleFor(e => e.Site)
                .MaximumLength(StaticProperties.URL_MAX_LENGTH)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Site));

            RuleFor(e => e.Facebook)
                .MaximumLength(StaticProperties.URL_MAX_LENGTH)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Facebook));

            RuleFor(e => e.Instagram)
                .MaximumLength(StaticProperties.URL_MAX_LENGTH)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Instagram));

            RuleFor(e => e.EventTime)
                .NotNull()
                .NotEmpty()
                .Must(e => e > DateTime.Now);

            RuleFor(e => e.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .Must(x => x % StaticProperties.PRICE_STEP == 0);

            //RuleFor(e => e.Images)
               //.NotNull()
               //.NotEmpty()
               //.Must(x => x.Count <= StaticProperties.IMAGE_MAX_COUNT);

        }
    }
}
