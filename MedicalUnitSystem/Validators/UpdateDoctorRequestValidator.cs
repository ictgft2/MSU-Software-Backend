using FluentValidation;
using MedicalUnitSystem.DTOs.Requests;
using static System.Net.Mime.MediaTypeNames;

namespace MedicalUnitSystem.Validators
{
    public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequestDto>
    {
        public UpdateDoctorRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter valid email");
        }
    }
}
