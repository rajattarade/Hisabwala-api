using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Expense.DeleteExpense
{
    public class DeleteExpenseCommandValidator : IValidator<DeleteExpenseCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public DeleteExpenseCommandValidator(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var partyValidationResult = await Utilities.ValidatePartyCode(request.PartyCode!, cancellationToken, _partyRepository);

            if (!partyValidationResult.Success)
                return partyValidationResult;

            var partyInfo = await _partyRepository.GetPartyAsync(request.PartyCode!, cancellationToken);
            if (!partyInfo.Expenses.Any(c => c.Id == request.ID))
            {
                return Result<bool>.Fail("Invalid Expense ID.");
            }

            return Result<bool>.Ok(true);
        }
    }
}
