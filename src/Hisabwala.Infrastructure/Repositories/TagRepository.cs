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
}
