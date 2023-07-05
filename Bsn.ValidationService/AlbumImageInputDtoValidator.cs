using FluentValidation;
using Model.Dto.Album;
using Model.Dto.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.ValidationService
{
    public class AlbumImageInputDtoValidator : AbstractValidator<AlbumImageInputDto>
    {
        public AlbumImageInputDtoValidator()
        {
            RuleFor(w => w.IdImages).NotEmpty().NotNull();
         

        }
    }
}
