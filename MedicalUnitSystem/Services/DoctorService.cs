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
            var newDoctor = new Doctor
            {
                Name = doctor.Name,
                GenderId = doctor.GenderId,
                Email = doctor.Email,
                Phone = doctor.Phone,
            };

            _repository.Doctors.Create(newDoctor);

            _repository.Save();

            var response = _mapper.Map<CreateDoctorResponseDto>(newDoctor);

            return Task.FromResult(Result<CreateDoctorResponseDto>.Success(response));
        }

        public void UpdateDoctor(int doctorId, UpdateDoctorRequestDto doctorDetails)
        {
            var existingDoctor = _repository.Doctors.FindByCondition(x => x.DoctorId == doctorId);

            var doctor = existingDoctor.FirstOrDefault();

            doctor.Name = doctorDetails.Name;
            doctor.Phone = doctorDetails.Phone;
            doctor.Email = doctorDetails.Email;
            doctor.GenderId = doctorDetails.GenderId;

            _repository.Doctors.Update(doctor);

            _repository.Save();
        }

        public Task<Result<GetDoctorResponseDto>> GetDoctor(int doctorId)
        {
            var existingDoctor = _repository.Doctors.FindByCondition(x => x.DoctorId == doctorId);

            if (existingDoctor == null)
            {
                return Task.FromResult(Result.Failure<GetDoctorResponseDto>($"Doctor with Id:{doctorId} not found"));
            }

            var response = _mapper.Map<GetDoctorResponseDto>(existingDoctor);

            return Task.FromResult(Result<GetDoctorResponseDto>.Success(response));
        }

        public Task<Result<List<GetDoctorResponseDto>>> GetDoctors()
        {
            var doctors = _repository.Doctors.FindAll();

            var response = _mapper.Map<List<GetDoctorResponseDto>>(doctors.ToList());

            return Task.FromResult(Result<List<GetDoctorResponseDto>>.Success(response));
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _repository.Doctors.DoctorExistsAsync(doctorId);
        }
    }
}
