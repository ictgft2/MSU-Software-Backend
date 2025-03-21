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

            CreateMap<Patient, CreatePatientResponseDto>();
            CreateMap<Patient, UpdatePatientResponseDto>();

            CreateMap<Consultation, CreateConsultationResponseDto>();

            CreateMap<Doctor, CreateDoctorResponseDto>();
            CreateMap<Doctor, UpdateDoctorResponseDto>();
            CreateMap<Doctor, GetDoctorResponseDto>();

            CreateMap<Gender, GetGenderResponseDto>();
            CreateMap<Gender, CreateGenderResponseDto>();
            CreateMap<Gender, UpdateGenderResponseDto>();            
            CreateMap<Admission, CreateAdmissionResponseDto>();     
            
            CreateMap<LaboratoryTestType, CreateLaboratoryTestTypeResponseDto>();            
            CreateMap<LaboratoryTestType, GetLaboratoryTestTypeResponseDto>();            
            
            CreateMap<LaboratoryTest, GetLaboratoryTestResponseDto>(); 
            CreateMap<LaboratoryTest, CreateLaboratoryTestResponseDto>(); 
            CreateMap<LaboratoryTest, UpdateLaboratoryTestResponseDto>(); 
            
            CreateMap<Consultation, GetConsultationResponseDto>();
            CreateMap<Consultation, CreateConsultationResponseDto>();
        }
    }
}
