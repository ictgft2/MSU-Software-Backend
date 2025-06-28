using FluentValidation;
using MedicalUnitSystem.DTOs.Requests;

namespace MedicalUnitSystem.Validators
{
    public class CreateConsultationRequestValidator : AbstractValidator<CreateConsultationRequestDto>
    {
        public CreateConsultationRequestValidator()
        {
            RuleFor(x => x.FollowupDate).GreaterThan(DateTime.UtcNow).WithMessage("Followup date can only be in the future");
        }
    }
}
