using FluentValidation;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Enums;

namespace QueueManagementApi.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        // Validate Email is not empty and is a valid email address
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        // Validate Name is not empty
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        // Validate Surname is not empty
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required.");

        // UserRole and ExhibitId logic:
        // If UserRole is set to Staff, then ExhibitId must be set
        When(x => x.UserRole == UserRole.Staff, () =>
        {
            RuleFor(x => x.ExhibitId)
                .NotEmpty().WithMessage("ExhibitId is required when UserRole is set to the Staff role.")
                .GreaterThan(0).WithMessage("ExhibitId must be greater than 0 when UserRole is set to the Staff role.");
        });
    }
}