using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Hisabwala.Core.Entities
{
    public class Expense
    {
        [BsonId] // tells MongoDB this is the _id field
        [BsonRepresentation(BsonType.ObjectId)] // allows using string instead of ObjectId
        public string Id { get; set; } = null!;
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaidBy { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
