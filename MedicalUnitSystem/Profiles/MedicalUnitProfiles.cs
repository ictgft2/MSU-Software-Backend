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
            CreateMap<Patient, CreatePatientResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));
            CreateMap<Consultation, CreateConsultationResponseDto>();
            CreateMap<Doctor, CreateDoctorResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));
            CreateMap<Gender, GetGenderResponseDto>();
            CreateMap<Gender, CreateGenderResponseDto>();
            CreateMap<Gender, UpdateGenderResponseDto>();
        }
    }
}
