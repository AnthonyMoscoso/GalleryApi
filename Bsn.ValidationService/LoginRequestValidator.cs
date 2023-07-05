
using FluentValidation;
using Model.Dto.Auth;

namespace Bsn.ValidationServices
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            RuleFor(w=> w).NotNull();
            RuleFor(w => w.Credential).NotEmpty().NotNull();
            RuleFor(w=> w.Identifier).NotEmpty().NotNull();
        }
    }
}
