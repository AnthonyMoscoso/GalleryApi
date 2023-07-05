using FluentValidation;
using Model.Dto.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.ValidationService
{
    public class AlbumInputDtoValidator : AbstractValidator<AlbumInputDto>
    {
        public AlbumInputDtoValidator()
        {
            RuleFor(w => w.Name).NotEmpty().NotNull();
          

        }
    }
}
