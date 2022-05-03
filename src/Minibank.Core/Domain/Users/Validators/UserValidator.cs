using FluentValidation;

namespace Minibank.Core.Domain.Users.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage("Login cannot be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");
        }
    }
}