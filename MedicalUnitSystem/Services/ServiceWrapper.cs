﻿using AutoMapper;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private IRepositoryWrapper _repository;
        private IPatientService _patient;
        private IConsultationService _consultation;
        private ILaboratoryTestService  _laboratoryTest;
        private ILaboratoryTestTypeService _laboratoryTestType;
        private IVitalsService _vitals;
        private IWaitingPatientService _waitingPatient;
        private IDoctorService _doctorService;
        private IGenderService _genderService;

        private readonly IMapper _mapper;


        public IPatientService Patient
        {
            get
            {
                if(_patient == null)
                {
                    _patient = new PatientService(_repository, _mapper);
                }

                return _patient;
            }
        }
        
        public IConsultationService Consultation
        {
            get
            {
                if(_consultation == null)
                {
                    _consultation = new ConsultationService(_repository, _mapper);
                }

                return _consultation;
            }
        }
        public ILaboratoryTestService LaboratoryTest
        {
            get
            {
                if(_laboratoryTest == null)
                {
                    _laboratoryTest = new LaboratoryTestService(_repository);
                }

                return _laboratoryTest;
            }
        }
        public ILaboratoryTestTypeService LaboratoryTestType
        {
            get
            {
                if(_laboratoryTestType == null)
                {
                    _laboratoryTestType = new LaboratoryTestTypeService(_repository);
                }

                return _laboratoryTestType;
            }
        }
        public IVitalsService Vitals
        {
            get
            {
                if(_vitals == null)
                {
                    _vitals = new VitalService(_repository, _mapper);
                }

                return _vitals;
            }
        }
        public IWaitingPatientService WaitingPatient
        {
            get
            {
                if(_waitingPatient == null)
                {
                    _waitingPatient = new WaitPatientService(_repository, _mapper);
                }

                return _waitingPatient;
            }
        }
        public IDoctorService Doctor
        {
            get
            {
                if(_doctorService == null)
                {
                    _doctorService = new DoctorService(_repository, _mapper);
                }

                return _doctorService;
            }
        }
        public IGenderService Gender
        {
            get
            {
                if(_genderService == null)
                {
                    _genderService = new GenderService(_repository, _mapper);
                }

                return _genderService;
            }
        }

        public ServiceWrapper(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
