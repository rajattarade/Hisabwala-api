using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Expense.EditExpense
{
    public class EditExpenseCommand : IRequest<Result<EditExpenseDTO>>
    {
        public string? PartyCode { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? PaidBy { get; set; }
        public string? Tag { get; set; }
    }
}
