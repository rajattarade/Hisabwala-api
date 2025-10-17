namespace Hisabwala.Core.Entities
{
    public class Contribution
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<string> Tags { get; set; } = new();
        public decimal Amount { get; set; }
    }
}
