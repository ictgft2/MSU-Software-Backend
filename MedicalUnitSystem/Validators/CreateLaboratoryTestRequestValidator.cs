using FluentValidation;
using MedicalUnitSystem.DTOs.Requests;

namespace MedicalUnitSystem.Validators
{
    public class CreateLaboratoryTestRequestValidator : AbstractValidator<CreateLaboratoryTestRequestDto>
    {
        public CreateLaboratoryTestRequestValidator() 
        {
            RuleFor(x => x.LaboratoryTestTypeId).GreaterThan(0);
            RuleFor(x => x.PatientId).GreaterThan(0);
        }
    }
}
