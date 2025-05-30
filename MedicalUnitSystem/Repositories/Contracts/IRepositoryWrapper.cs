﻿using MedicalUnitSystem.Data;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IConsultationRepository Consultations { get; }
        IPatientRepository Patients { get; }
        ILaboratoryTestRepository LaboratoryTests { get; }
        ILaboratoryTestTypeRepository LaboratoryTestTypes { get; }
        IVitalsRepository Vitals { get; }
        IWaitingQueueRepository WaitingQueues { get; }
        IPrescriptionRepository Prescriptions { get; }
        IDoctorRepository Doctors { get; }
        IGenderRepository Genders { get; }
        IAdmissionRepository Admissions { get; }
        HospitalContext Context { get; }
        void Save();
    }
}
