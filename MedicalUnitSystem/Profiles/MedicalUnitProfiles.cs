using AutoMapper;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Profiles
{
    public class MedicalUnitProfiles : Profile
    {
        public MedicalUnitProfiles()
        {
            CreateMap<Vital, VitalsResponseDto>();
            CreateMap<Patient, PatientResponseDto>();
        }
    }
}
