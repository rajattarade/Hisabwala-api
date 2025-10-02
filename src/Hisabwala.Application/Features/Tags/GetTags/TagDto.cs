namespace Hisabwala.Application.Features.Tags.GetTags;

public class TagDto
{
    public int Id { get; set; }
    public string TagName { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
}