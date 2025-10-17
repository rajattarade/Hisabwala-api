using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisabwala.Core.Entities
{
    public class Contribution
    {
        public string Name { get; set; } = default!;
        public List<string> Tags { get; set; } = new();
        public decimal Amount { get; set; }
    }
}
