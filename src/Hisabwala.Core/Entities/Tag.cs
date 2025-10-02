namespace Hisabwala.Core.Entities;

public class Tag
{
    public int Id { get; set; }
    public string TagName { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}