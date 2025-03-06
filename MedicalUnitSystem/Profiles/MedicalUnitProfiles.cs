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
            CreateMap<Patient, UpdatePatientResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));
            CreateMap<Patient, GetPatientResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));

            CreateMap<Consultation, CreateConsultationResponseDto>();

            CreateMap<Doctor, CreateDoctorResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));
            CreateMap<Doctor, UpdateDoctorResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));
            CreateMap<Doctor, GetDoctorResponseDto>()
                .ForMember(x => x.Gender, s => s.MapFrom(src => src.Gender.GenderName));

            CreateMap<Gender, GetGenderResponseDto>();
            CreateMap<Gender, CreateGenderResponseDto>();
            CreateMap<Gender, UpdateGenderResponseDto>();            
            CreateMap<Admission, CreateAdmissionResponseDto>();            
        }
    }
}
