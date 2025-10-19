using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hisabwala.Core.Entities
{
    public class Party
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string PartyCode { get; set; } = default!;
        public string PartyName { get; set; } = default!;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now.ToUniversalTime();

        [BsonElement("tags")]
        public List<string> Tags { get; private set; } = new();        
        [BsonElement("expenses")]
        public List<Expense> Expenses { get; private set; } = new();
        [BsonElement("contributions")]
        public List<Contribution> Contributions { get; private set; } = new();

        public void UpdateTags(List<string> tagsList)
        {
            Tags = tagsList;
        }
    }
}
