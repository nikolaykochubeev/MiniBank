using FluentValidation;

namespace Minibank.Core.Domain.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccountModel>
    {
        public BankAccountValidator()
        {
            RuleFor(x => x.IsActive).Equal(true).WithMessage($"Account is not active");
        }
    }
}