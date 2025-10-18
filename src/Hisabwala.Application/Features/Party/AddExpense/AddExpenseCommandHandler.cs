using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Party.AddExpense
{
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, Result<AddExpenseDTO>>
    {
        private readonly IPartyRepository _partyRepository;

        public AddExpenseCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<AddExpenseDTO>> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            Core.Entities.Party party = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);

            var expense = new Core.Entities.Expense
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Amount = request.Amount,
                PaidBy = request.PaidBy,
                Tag = request.Tag
            };

            party.Expenses.Add(expense);

            await _partyRepository.UpdatePartyAsync(party, cancellationToken);

            var expenseDTO = new AddExpenseDTO
            {
                Id = expense.Id
            };

            return Result<AddExpenseDTO>.Ok(expenseDTO);
        }
    }
}
