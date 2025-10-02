using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagsAsync();
}