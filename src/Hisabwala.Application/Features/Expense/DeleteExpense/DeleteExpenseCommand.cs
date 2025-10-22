using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Expense.DeleteExpense
{
    public class DeleteExpenseCommand : IRequest<Result<DeleteExpenseDTO>>
    {
        public string? PartyCode { get; set; }
        public string? ID { get; set; }
    }
}
