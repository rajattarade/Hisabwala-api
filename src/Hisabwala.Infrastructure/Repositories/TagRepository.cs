using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Hisabwala.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hisabwala.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags
            .OrderBy(t => t.TagName)
            .ToListAsync();
    }

    public async Task<Tag> AddTagAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag?> GetTagIfExistsAsync(string tagName)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.TagName.Trim().ToLower() == tagName.Trim().ToLower());
    }
}
