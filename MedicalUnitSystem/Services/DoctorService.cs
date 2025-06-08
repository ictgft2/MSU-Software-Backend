using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Helpers.Enums;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IPropertyCheckingService _propertyCheckingService;
        private readonly IMapper _mapper;
        public DoctorService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckingService = propertyCheckingService;
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
            var existingDoctorQuery = _repository.Doctors.FindByCondition(x => x.DoctorId == doctorId);

            var doctor = existingDoctorQuery.FirstOrDefault();

            if (doctor == null)
            {
                return Task.FromResult(Result.Failure<GetDoctorResponseDto>($"Doctor with Id:{doctorId} not found"));
            }

            var response = _mapper.Map<GetDoctorResponseDto>(doctor);

            return Task.FromResult(Result<GetDoctorResponseDto>.Success(response));
        }

        public async Task<PagedList<GetDoctorResponseDto>> GetDoctors(GetPaginatedDataRequestDto query)
        {
            IQueryable<Doctor> doctorsQuery = _repository.Doctors.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                doctorsQuery = _repository.Doctors
                    .FindByCondition(d => d.Name.Contains(query.searchTerm) || d.Email.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<Doctor>(query.sortColumn);

            if(propertyInfo is not null)
            {
                doctorsQuery = doctorsQuery.OrderByProperty(propertyInfo, query.sortOrder);
            }

            var doctorResponsesQuery = doctorsQuery
                .Select(d => new GetDoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Email = d.Email,
                    Phone = d.Phone
                });

            var doctors = await PagedList<GetDoctorResponseDto>.CreateAsync(doctorResponsesQuery, query.page, query.pageSize);

            return doctors;
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _repository.Doctors.DoctorExistsAsync(doctorId);
        }
    }
}
