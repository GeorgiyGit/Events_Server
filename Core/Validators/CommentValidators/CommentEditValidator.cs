using Core.DTOs.CommentDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.CommentValidators
{
    internal class CommentEditValidator : AbstractValidator<CommentEditDTO>
    {
        public CommentEditValidator()
        {
            this.RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
