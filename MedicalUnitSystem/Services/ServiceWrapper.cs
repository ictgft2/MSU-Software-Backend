using AutoMapper;
using MedicalUnitSystem.Helpers;
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
        private IDoctorService _doctor;
        private IGenderService _gender;
        private IAdmissionService _admission;

        private readonly IMapper _mapper;
        private readonly IPropertyCheckingService _propertyCheckingService;

        public IAdmissionService Admission
        {
            get
            {
                if (_admission == null)
                {
                    _admission = new AdmissionService(_repository, _mapper);
                }

                return _admission;
            }
        }

        public IPatientService Patient
        {
            get
            {
                if(_patient == null)
                {
                    _patient = new PatientService(_repository, _mapper, _propertyCheckingService);
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
                    _consultation = new ConsultationService(_repository, _mapper, _propertyCheckingService);
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
                    _laboratoryTest = new LaboratoryTestService(_repository, _mapper, _propertyCheckingService);
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
                    _laboratoryTestType = new LaboratoryTestTypeService(_repository, _mapper, _propertyCheckingService);
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
                if(_doctor == null)
                {
                    _doctor = new DoctorService(_repository, _mapper, _propertyCheckingService);
                }

                return _doctor;
            }
        }
        public IGenderService Gender
        {
            get
            {
                if(_gender == null)
                {
                    _gender = new GenderService(_repository, _mapper);
                }

                return _gender;
            }
        }       

        public ServiceWrapper(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository;
            _mapper = mapper;
            _propertyCheckingService = propertyCheckingService;
        }
    }
}
