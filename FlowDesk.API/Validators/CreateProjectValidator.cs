using FlowDesk.Application.DTO;
using FluentValidation;

namespace FlowDesk.API.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name cannot be empty.")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters.");
    }
}