using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisabwala.Core.Entities
{
    public class Expense
    {
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaidBy { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
