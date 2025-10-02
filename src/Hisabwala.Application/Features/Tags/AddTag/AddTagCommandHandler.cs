using MediatR;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Hisabwala.Application.Features.Tags.GetTags;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Tags.AddTag;

public class AddTagCommandHandler : IRequestHandler<AddTagCommand, Result<TagDto>>
{
    private readonly ITagRepository _tagRepository;

    public AddTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<TagDto>> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetTagIfExistsAsync(request.TagName);

        if(tag == null)
        {
            tag = new Tag
            {
                TagName = request.TagName
            };

            tag = await _tagRepository.AddTagAsync(tag);
        }
        
        // Map to DTO
        var tagDto = new TagDto
        {
            Id = tag.Id,
            TagName = tag.TagName,
            CreatedDateTime = tag.CreatedDateTime
        };

        return Result<TagDto>.Ok(tagDto);
    }
}
