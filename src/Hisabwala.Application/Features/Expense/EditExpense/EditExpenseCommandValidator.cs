using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Expense.EditExpense
{
    public class EditExpenseCommandValidator : IValidator<EditExpenseCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public EditExpenseCommandValidator(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(EditExpenseCommand request, CancellationToken cancellationToken)
        {
            var partyValidationResult = await Utilities.ValidatePartyCode(request.PartyCode!, cancellationToken, _partyRepository);

            if (!partyValidationResult.Success)
                return partyValidationResult;

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<bool>.Fail("Expense name is required.");

            if (request.Name.Length > 100)
                return Result<bool>.Fail("Expense name cannot exceed 100 characters.");

            if (request.Amount <= 0)
                return Result<bool>.Fail("Expense amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(request.PaidBy))
                return Result<bool>.Fail("PaidBy is required.");

            if (request.PaidBy.Length > 100)
                return Result<bool>.Fail("PaidBy cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(request.Tag))
                return Result<bool>.Fail("Tag is required.");

            if (request.Tag.Length > 50)
                return Result<bool>.Fail("Tag cannot exceed 50 characters.");

            var partyInfo = await _partyRepository.GetPartyAsync(request.PartyCode!, cancellationToken);
            if (!partyInfo.Expenses.Any(c => c.Id == request.Id))
            {
                return Result<bool>.Fail("Invalid Expense ID.");
            }

            if (partyInfo.Expenses.Any(c => c.Name.ToLower() == request.Name.ToLower() && c.Id != request.Id))
            {
                return Result<bool>.Fail("Expense with the same name already exists.");
            }

            return Result<bool>.Ok(true);
        }
    }
}
