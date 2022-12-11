using Core.DTOs.PlaceDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.PlaceValidators
{
    internal class PlaceEditValidator : AbstractValidator<PlaceEditDTO>
    {
        public PlaceEditValidator()
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty();

			RuleFor(e => e.Name)
				.NotNull()
				.NotEmpty()
				.MinimumLength(StaticProperties.USER_NAME_MIN_LENGTH)
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

			RuleFor(e => e.Route)
				.NotNull()
				.NotEmpty()
				.MaximumLength(500);

			RuleFor(e => e.Facebook)
				.MaximumLength(StaticProperties.URL_MAX_LENGTH)
				.Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
				.When(x => !string.IsNullOrEmpty(x.Facebook));

			RuleFor(e => e.Instagram)
				.MaximumLength(StaticProperties.URL_MAX_LENGTH)
				.Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
				.When(x => !string.IsNullOrEmpty(x.Instagram));

			RuleFor(e => e.GoogleMaps)
				.NotNull()
				.NotEmpty()
				.MaximumLength(StaticProperties.URL_MAX_LENGTH)
				.Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
				.When(x => !string.IsNullOrEmpty(x.GoogleMaps));
		}
    }
}
