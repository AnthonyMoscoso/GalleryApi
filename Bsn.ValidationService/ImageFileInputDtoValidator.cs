﻿using FluentValidation;
using Model.Dto.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.ValidationService
{
    public class ImageFileInputDtoValidator : AbstractValidator<ImageFileInputDto>
    {
        public ImageFileInputDtoValidator()
        {
            RuleFor(w => w.Name).NotEmpty().NotNull();
            RuleFor(w => w.Description).MaximumLength(600);
 
        }
    }
}
