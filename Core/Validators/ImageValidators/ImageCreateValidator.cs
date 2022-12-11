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
    internal class ImageCreateValidator : AbstractValidator<ImageCreateDTO>
    {
        public ImageCreateValidator()
        {

        }
    }
}
