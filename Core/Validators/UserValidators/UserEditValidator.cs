using Core.DTOs.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.UserValidators
{
    internal class UserEditValidator : AbstractValidator<UserEditDTO>
    {
        public UserEditValidator()
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
