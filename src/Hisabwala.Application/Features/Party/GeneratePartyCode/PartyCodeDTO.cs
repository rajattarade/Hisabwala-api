using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisabwala.Application.Features.Party.GeneratePartyCode
{
    public class PartyCodeDTO
    {
        public string Id { get; set; } = null!;
        public string PartyName { get; set; } = string.Empty;
        public string PartyCode { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; }
    }
}
