using MediatR;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Features.Tags;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<TagDto>>
{
    private readonly ITagRepository _tagRepository;

    public GetTagsQueryHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAllTagsAsync();

        return tags.Select(t => new TagDto
        {
            Id = t.Id,
            TagName = t.TagName,
            CreatedDateTime = t.CreatedDateTime
        }).ToList();
    }
}
