using FluentValidation;
using Model.Dto.User;

namespace Bsn.ValidationService
{
    public class UserInputDtoValidator : AbstractValidator<UserInputDto>
    {
        public UserInputDtoValidator()
        {
     
            RuleFor(w => w.Username).NotEmpty().NotNull();          
            RuleFor(w => w.Name).NotEmpty().NotNull();

                
        }

   
    }
}
