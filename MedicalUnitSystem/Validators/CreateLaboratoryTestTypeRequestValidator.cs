using FluentValidation;
using MedicalUnitSystem.DTOs.Requests;

namespace MedicalUnitSystem.Validators
{
    public class CreateLaboratoryTestTypeRequestValidator : AbstractValidator<CreateLaboratoryTestTypeRequestDto>
    {
        public CreateLaboratoryTestTypeRequestValidator() 
        {
            RuleFor(x => x.LaboratoryTestTypeName).NotEmpty().WithMessage("Laboratory Test Type should not be null");
        }
    }
}
