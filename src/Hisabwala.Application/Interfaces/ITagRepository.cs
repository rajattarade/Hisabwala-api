using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagsAsync();
    Task<Tag> AddTagAsync(Tag tag);
    Task<Tag?> GetTagIfExistsAsync(string tagName);
}