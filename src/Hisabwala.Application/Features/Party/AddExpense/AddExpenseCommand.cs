using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.AddExpense
{
    public class AddExpenseCommand : IRequest<Result<AddExpenseDTO>>
    {
        public string PartyCode { get; set; } = null!;
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaidBy { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
