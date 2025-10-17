namespace Hisabwala.Core.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaidBy { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
