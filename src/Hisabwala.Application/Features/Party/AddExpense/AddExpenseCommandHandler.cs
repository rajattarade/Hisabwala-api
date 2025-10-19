using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using Hisabwala.Core.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hisabwala.Application.Features.Party.AddExpense
{
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, Result<AddExpenseDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private Core.Entities.Party partyInDatabase;

        public AddExpenseCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<AddExpenseDTO>> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);

            var expense = new Expense
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name.FirstCharToUpper(),
                Amount = request.Amount,
                PaidBy = request.PaidBy.FirstCharToUpper(),
                Tag = request.Tag.FirstCharToUpper()
            };

            AddExpense(expense);

            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var expenseDTO = new AddExpenseDTO
            {
                Id = expense.Id
            };

            return Result<AddExpenseDTO>.Ok(expenseDTO);
        }

        private void AddExpense(Expense expense)
        {
            partyInDatabase.Expenses.Add(expense);
            UpdateTags();
            AddContributorIfNeeded(expense.PaidBy, expense.Tag);
            partyInDatabase.UpdateContributions();
        }

        private void AddContributorIfNeeded(string contributorName, string contributionTag)
        {
            if (!partyInDatabase.Contributions.Any(c => c.Name == contributorName))
            {
                partyInDatabase.Contributions.Add(new Contribution
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
            partyInDatabase.UpdateTags(partyInDatabase.Expenses.Select(e => e.Tag.FirstCharToUpper()).Distinct().ToList());
        }
    }
}
