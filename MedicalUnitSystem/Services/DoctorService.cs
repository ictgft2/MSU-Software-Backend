using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public DoctorService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<CreateDoctorResponseDto>> CreateDoctor(CreateDoctorRequestDto doctor)
        {
            var gender = _repository.Gender.FindByCondition(x => x.GenderId == doctor.GenderId);

            if (gender == null)
            {
                return Task.FromResult(Result.Failure<CreateDoctorResponseDto>($"Gender with id: {doctor.GenderId} not found"));
            }

            var newDoctor = new Doctor
            {
                Name = doctor.Name,
                Gender = gender.FirstOrDefault(),
                Email = doctor.Email,
                Phone = doctor.Phone,
            };

            _repository.Doctor.Create(newDoctor);

            _repository.Save();

            var response = _mapper.Map<CreateDoctorResponseDto>(newDoctor);

            return Task.FromResult(Result<CreateDoctorResponseDto>.Success(response));
        }

        public Task<Result<UpdateDoctorResponseDto>> UpdateDoctor(int doctorId, UpdateDoctorRequestDto doctorDetails)
        {
            var existingDoctor = _repository.Doctor.FindByCondition(x => x.DoctorId == doctorId);

            if (existingDoctor != null)
            {
                return Task.FromResult(Result.Failure<UpdateDoctorResponseDto>($"Doctor with Id:{doctorId} not found"));
            }

            var doctor = existingDoctor.FirstOrDefault();

            var gender = _repository.Gender.FindByCondition(x => x.GenderId == doctorDetails.GenderId);

            if (gender.FirstOrDefault() == null)
            {
                return Task.FromResult(Result.Failure<UpdateDoctorResponseDto>($"Gender with id: {doctorDetails.GenderId} not found"));
            }

            doctor.Name = doctorDetails.Name;
            doctor.Phone = doctorDetails.Phone;
            doctor.Email = doctorDetails.Email;
            doctor.Gender = gender.FirstOrDefault();

            _repository.Doctor.Update(doctor);

            _repository.Save();

            var response = _mapper.Map<UpdateDoctorResponseDto>(doctor);

            return Task.FromResult(Result<UpdateDoctorResponseDto>.Success(response));
        }

        public Task<Result<GetDoctorResponseDto>> GetDoctor(int doctorId)
        {
            var existingDoctor = _repository.Doctor.FindByCondition(x => x.DoctorId == doctorId);

            if (existingDoctor != null)
            {
                return Task.FromResult(Result.Failure<GetDoctorResponseDto>($"Doctor with Id:{doctorId} not found"));
            }

            var response = _mapper.Map<GetDoctorResponseDto>(existingDoctor);

            return Task.FromResult(Result<GetDoctorResponseDto>.Success(response));
        }
        public Task<Result<List<GetDoctorResponseDto>>> GetDoctors()
        {
            var doctors = _repository.Doctor.FindAll();

            var response = _mapper.Map<List<GetDoctorResponseDto>>(doctors.ToList());

            return Task.FromResult(Result<List<GetDoctorResponseDto>>.Success(response));
        }
    }
}
