using Core.DTOs.EventDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.EventValidators
{
    internal class EventEditValidator : AbstractValidator<EventEditDTO>
    {
        public EventEditValidator()
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
