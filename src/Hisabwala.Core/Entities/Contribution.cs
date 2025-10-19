namespace Hisabwala.Core.Entities
{
    public class Contribution
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<string> Tags { get; set; } = new();
        public decimal Amount { get; set; }
    }
}
