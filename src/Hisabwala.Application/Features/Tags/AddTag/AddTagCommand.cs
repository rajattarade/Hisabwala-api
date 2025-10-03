using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Tags.AddTag;

public class AddTagCommand : IRequest<Result<TagDto>>
{
    public string TagName { get; set; } = null!;
}