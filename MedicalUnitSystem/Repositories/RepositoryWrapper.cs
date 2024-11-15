using MedicalUnitSystem.Data;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private HospitalContext _context;
        private IConsultationRepository _consultation;
        private IPatientRepository _patient;

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
