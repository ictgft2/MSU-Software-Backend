using FluentValidation;
using MedicalUnitSystem.DTOs.Requests;

namespace MedicalUnitSystem.Validators
{
    public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequestDto>
    {
        private const int MinAge = 1;
        public CreatePatientRequestValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter valid email");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Please enter Patient's date of birth")
                .LessThanOrEqualTo(DateTime.Today)
                    .WithMessage("Date of birth cannot be in the future")
                .Must(BeOfMinimumAge)
                    .WithMessage($"You must be at least {MinAge} years old");
        }
        private bool BeOfMinimumAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age))
            {
                age--;
            }
            return age >= MinAge;
        }
    }
}
