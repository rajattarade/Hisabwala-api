using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Expense.EditExpense
{
    public class EditExpenseCommandHandler : IRequestHandler<EditExpenseCommand, Result<EditExpenseDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private Core.Entities.Party? partyInDatabase;

        public EditExpenseCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<EditExpenseDTO>> Handle(EditExpenseCommand request, CancellationToken cancellationToken)
        {
            partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode!, cancellationToken);
            var expense = partyInDatabase.Expenses.FirstOrDefault(c => c.Id == request.Id);
            if (expense == null)
            {
                return Result<EditExpenseDTO>.Fail("Expense not found.");
            }

            expense.Name = request.Name!.FirstCharToUpper();
            expense.Amount = request.Amount;
            expense.PaidBy = request.PaidBy!.FirstCharToUpper();
            expense.Tag = request.Tag!.FirstCharToUpper();

            UpdateTags();
            AddContributorIfNeeded(expense.PaidBy, expense.Tag);
            partyInDatabase.UpdateContributions();

            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var expenseDTO = new EditExpenseDTO
            {
                Id = expense.Id
            };

            return Result<EditExpenseDTO>.Ok(expenseDTO);
        }

        private void AddContributorIfNeeded(string contributorName, string contributionTag)
        {
            if (!partyInDatabase!.Contributions.Any(c => c.Name == contributorName))
            {
                partyInDatabase.Contributions.Add(new Core.Entities.Contribution
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = contributorName,
                    Tags = new List<string>() { contributionTag },
                });
            }
            else
            {
                var contributor = partyInDatabase.Contributions.First(c => c.Name == contributorName);
                if (!contributor.Tags.Contains(contributionTag))
                {
                    contributor.Tags.Add(contributionTag);
                }
            }
        }

        private void UpdateTags()
        {
            partyInDatabase!.UpdateTags(partyInDatabase.Expenses.Select(e => e.Tag.FirstCharToUpper()).Distinct().ToList());
        }
    }
}
