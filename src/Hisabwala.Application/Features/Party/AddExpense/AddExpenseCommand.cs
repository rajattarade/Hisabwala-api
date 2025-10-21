using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.AddExpense
{
    public class AddExpenseCommand : IRequest<Result<AddExpenseDTO>>
    {
        public string? PartyCode { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? PaidBy { get; set; }
        public string? Tag { get; set; }
    }
}
