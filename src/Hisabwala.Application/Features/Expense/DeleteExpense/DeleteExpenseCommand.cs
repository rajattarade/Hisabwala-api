using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.AddExpense;
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
