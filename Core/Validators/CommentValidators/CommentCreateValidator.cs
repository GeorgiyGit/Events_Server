using Core.DTOs.CommentDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators.CommentValidators
{
    internal class CommentCreateValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateValidator()
        {
            this.RuleFor(e => e.Text)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StaticProperties.COMMENT_MAX_LENGTH);
        }
    }
}
