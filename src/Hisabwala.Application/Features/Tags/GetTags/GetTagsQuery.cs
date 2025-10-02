using Hisabwala.Application.Features.Tags.GetTags;
using MediatR;

public record GetTagsQuery() : IRequest<List<TagDto>>;
