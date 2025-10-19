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


        [BsonElement("tags")]
        public List<string> Tags { get; private set; } = new();        
        [BsonElement("expenses")]
        public List<Expense> Expenses { get; private set; } = new();
        [BsonElement("contributions")]
        public List<Contribution> Contributions { get; private set; } = new();

        public void AddContribution(Contribution contri)
        {
            AddContributorIfNeeded(contri);
            UpdateContributions();
        }

        private void AddContributorIfNeeded(Contribution contri)
        {
            if (!Contributions.Any(c => c.Name == contri.Name))
            {
                Contributions.Add(new Contribution
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = contri.Name,
                    Tags = contri.Tags,
                });
            }
            else
            {
                Contributions.First(c => c.Name == contri.Name).Tags.AddRange(contri.Tags);
            }
        }

        public void AddExpense(Expense expense)
        {
            Expenses.Add(expense);
            UpdateTags();
            AddContributorIfNeeded(expense);
            UpdateContributions();
        }

        private void AddContributorIfNeeded(Expense expense)
        {
            if(!Contributions.Any(c => c.Name == expense.PaidBy))
            {
                Contributions.Add(new Contribution
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = expense.PaidBy,
                    Tags = new List<string>() { expense.Tag },
                });
            }
            else
            {
                Contributions.First(c => c.Name == expense.PaidBy).Tags.Add(expense.Tag);
            }
        }

        private void UpdateContributions()
        {
            Dictionary<string, decimal> tagAmounts = new();
            foreach (var expense in Expenses)
            {
                if (tagAmounts.ContainsKey(expense.Tag))
                {
                    tagAmounts[expense.Tag] += expense.Amount;
                }
                else
                {
                    tagAmounts[expense.Tag] = expense.Amount;
                }
            }

            Dictionary<string, decimal> peoplePerTag = new();
            foreach (var contribution in Contributions)
            {
                contribution.Amount = 0;
                foreach (var tag in contribution.Tags.Distinct())
                {
                    if (peoplePerTag.ContainsKey(tag))
                    {
                        peoplePerTag[tag] += 1;
                    }
                    else
                    {
                        peoplePerTag[tag] = 1;
                    }
                }
            }

            foreach (var tagAmount in tagAmounts)
            {
                var tag = tagAmount.Key;
                var totalAmount = tagAmount.Value;
                var peopleCount = peoplePerTag[tag];
                var amountPerPerson = Math.Round(totalAmount / peopleCount);
                foreach (var contribution in Contributions)
                {
                    if (contribution.Tags.Contains(tag))
                    {
                        contribution.Amount += amountPerPerson;
                    }
                }
            }
        }

        private void UpdateTags()
        {
            Tags = Expenses.Select(e => e.Tag).Distinct().ToList();
        }
    }
}
