using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hisabwala.Core.Entities
{
    public class Party
    {
        [BsonId] // tells MongoDB this is the _id field
        [BsonRepresentation(BsonType.ObjectId)] // allows using string instead of ObjectId
        public string Id { get; set; } = null!;
        public string PartyCode { get; set; } = default!;
        public string PartyName { get; set; } = default!;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now.ToUniversalTime();

        public List<Expense> Expenses { get; set; } = new();
        public List<Contribution> Contributions { get; set; } = new();
    }
}
