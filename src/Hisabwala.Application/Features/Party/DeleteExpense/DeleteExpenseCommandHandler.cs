using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Features.Party.EditContribution;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using Hisabwala.Core.Entities;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Party.DeleteExpense
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Result<DeleteExpenseDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private Core.Entities.Party partyInDatabase;

        public DeleteExpenseCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<DeleteExpenseDTO>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode!, cancellationToken);
            var expense = partyInDatabase.Expenses.FirstOrDefault(c => c.Id == request.ID);
            if (expense == null)
            {
                return Result<DeleteExpenseDTO>.Fail("Expense not found.");
            }

            partyInDatabase.Expenses.Remove(expense);
            UpdateTags();
            partyInDatabase.UpdateContributions();

            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var expenseDTO = new DeleteExpenseDTO
            {
                Id = expense.Id
            };

            return Result<DeleteExpenseDTO>.Ok(expenseDTO);
        }

        private void UpdateTags()
        {
            partyInDatabase.UpdateTags(partyInDatabase.Expenses.Select(e => e.Tag.FirstCharToUpper()).Distinct().ToList());
        }
    }
}
