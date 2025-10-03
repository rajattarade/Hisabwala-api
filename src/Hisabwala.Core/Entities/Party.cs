using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisabwala.Core.Entities
{
    public class Party
    {
        public int Id { get; set; }
        public string PartyName { get; set; } = null!;
        public string PartyCode { get; set; } = null!;
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
