using Hisabwala.Application.Features.Tags;
using MediatR;

public record GetTagsQuery() : IRequest<List<TagDto>>;
