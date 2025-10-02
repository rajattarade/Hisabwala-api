using System.Text.RegularExpressions;
using Hisabwala.Application.Features.Tags.AddTag;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;

public class AddTagValidator : IValidator<AddTagCommand>
{
    private readonly ITagRepository _tagRepository;

    public AddTagValidator(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<bool>> ValidateAsync(AddTagCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.TagName))
            return Result<bool>.Fail("Tag name is required.");

        if (request.TagName.Length > 255)
            return Result<bool>.Fail("Tag name cannot exceed 255 characters.");

        if (!Regex.IsMatch(request.TagName, @"^[a-zA-Z0-9]+$"))
            return Result<bool>.Fail("Tag name must only contain letters and numbers, without spaces or special characters.");

        return Result<bool>.Ok(true);
    }
}