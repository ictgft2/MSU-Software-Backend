using MedicalUnitSystem.Data;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private HospitalContext _context;
        private IConsultationRepository _consultation;
        private IPatientRepository _patient;
        private ILaboratoryTestRepository _laboratoryTest;
        private ILaboratoryTestTypeRepository _laboratoryTestType;

        public IConsultationRepository Consultation
        {
            get
            {
                if(_consultation == null)
                {
                    _consultation = new ConsultationRepository(_context);
                }

                return _consultation;
            }
        }
        
        public IPatientRepository Patient
        {
            get
            {
                if(_patient == null)
                {
                    _patient = new PatientRepository(_context);
                }

                return _patient;
            }
        }
        public ILaboratoryTestTypeRepository LaboratoryTestType
        {
            get
            {
                if(_laboratoryTestType == null)
                {
                    _laboratoryTestType = new LaboratoryTestTypeRepository(_context);
                }

                return _laboratoryTestType;
            }
        }
        public ILaboratoryTestRepository LaboratoryTest
        {
            get
            {
                if(_laboratoryTest == null)
                {
                    _laboratoryTest = new LaboratoryTestRepository(_context);
                }

                return _laboratoryTest;
            }
        }

        public RepositoryWrapper(HospitalContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
