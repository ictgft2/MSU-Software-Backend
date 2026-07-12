CREATE TABLE dbo.Patients (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    FullName nvarchar(200) NOT NULL,
    Age int NOT NULL,
    Sex nvarchar(1) NOT NULL,
    Phone nvarchar(40) NOT NULL,
    Address nvarchar(500) NOT NULL,
    NextOfKinName nvarchar(200) NOT NULL,
    NextOfKinPhone nvarchar(40) NOT NULL,
    NextOfKinRelationship nvarchar(100) NOT NULL,
    CreatedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.Encounters (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    PatientId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Patients(Id),
    AdmissionType nvarchar(30) NOT NULL,
    Status nvarchar(40) NOT NULL,
    ArrivalMode nvarchar(30) NOT NULL,
    ChiefComplaint nvarchar(1000) NOT NULL,
    RegisteredBy uniqueidentifier NOT NULL,
    AdmittedAt datetimeoffset NOT NULL,
    DischargedAt datetimeoffset NULL,
    CreatedAt datetimeoffset NOT NULL,
    UpdatedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.VitalSigns (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    RecordedBy uniqueidentifier NOT NULL,
    BloodPressureSystolic int NULL,
    BloodPressureDiastolic int NULL,
    PulseRate int NULL,
    Temperature decimal(5,2) NULL,
    Spo2 int NULL,
    RespiratoryRate int NULL,
    Weight decimal(8,2) NULL,
    Notes nvarchar(1000) NULL,
    RecordedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.ConsultationNotes (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    DoctorId uniqueidentifier NOT NULL,
    Diagnosis nvarchar(max) NOT NULL,
    ClinicalNotes nvarchar(max) NOT NULL,
    RequiresLab bit NOT NULL,
    RequiresDressing bit NOT NULL,
    IsReferral bit NOT NULL,
    ReferralFacility nvarchar(250) NULL,
    ReferralReason nvarchar(1000) NULL,
    ConsultedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.Prescriptions (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    ConsultationNoteId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.ConsultationNotes(Id),
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    DrugName nvarchar(200) NOT NULL,
    Dosage nvarchar(100) NOT NULL,
    Frequency nvarchar(100) NOT NULL,
    Duration nvarchar(100) NOT NULL,
    Route nvarchar(30) NOT NULL,
    Instructions nvarchar(1000) NULL,
    Status nvarchar(30) NOT NULL,
    IssuedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.Dispensings (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    PrescriptionId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Prescriptions(Id),
    PharmacistId uniqueidentifier NOT NULL,
    DrugName nvarchar(200) NOT NULL,
    QuantityDispensed int NOT NULL,
    BatchNumber nvarchar(100) NOT NULL,
    ExpiryDate date NOT NULL,
    Notes nvarchar(1000) NULL,
    DispensedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.DrugHandovers (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    DispensingId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Dispensings(Id),
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    ProtocolOfficerId uniqueidentifier NULL,
    PatientNameVerified bit NOT NULL DEFAULT 0,
    DrugListVerified bit NOT NULL DEFAULT 0,
    DosageCounsellingDone bit NOT NULL DEFAULT 0,
    DurationCounsellingDone bit NOT NULL DEFAULT 0,
    CounsellingNotes nvarchar(1000) NULL,
    HandoverAt datetimeoffset NULL
);

CREATE TABLE dbo.LabRequests (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    ConsultationNoteId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.ConsultationNotes(Id),
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    TestName nvarchar(200) NOT NULL,
    ClinicalIndication nvarchar(1000) NOT NULL,
    Status nvarchar(30) NOT NULL,
    RequestedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.LabResults (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    LabRequestId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.LabRequests(Id),
    ScientistId uniqueidentifier NOT NULL,
    TestName nvarchar(200) NOT NULL,
    Findings nvarchar(max) NOT NULL,
    Conclusion nvarchar(max) NOT NULL,
    [Values] nvarchar(max) NOT NULL,
    CompletedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.DressingOrders (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    ConsultationNoteId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.ConsultationNotes(Id),
    EncounterId uniqueidentifier NOT NULL FOREIGN KEY REFERENCES dbo.Encounters(Id),
    Instructions nvarchar(max) NOT NULL,
    Status nvarchar(30) NOT NULL,
    PerformedBy uniqueidentifier NULL,
    ProcedureNotes nvarchar(max) NULL,
    CompletedAt datetimeoffset NULL,
    CreatedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.ContactTraces (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    EncounterId uniqueidentifier NOT NULL UNIQUE FOREIGN KEY REFERENCES dbo.Encounters(Id),
    RecordedBy uniqueidentifier NOT NULL,
    NextOfKinName nvarchar(200) NOT NULL,
    NextOfKinPhone nvarchar(40) NOT NULL,
    NextOfKinRelationship nvarchar(100) NOT NULL,
    ResidentialAddress nvarchar(500) NOT NULL,
    WorkplaceAddress nvarchar(500) NOT NULL,
    DischargeNotes nvarchar(max) NOT NULL,
    ReferralDestination nvarchar(250) NULL,
    RecordedAt datetimeoffset NOT NULL
);

CREATE TABLE dbo.ServiceTimeWindows (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    [Date] date NOT NULL UNIQUE,
    ColdCaseOpenTime time NOT NULL,
    ColdCaseCloseTime time NOT NULL,
    CreatedBy uniqueidentifier NOT NULL,
    CreatedAt datetimeoffset NOT NULL
);

GO

CREATE VIEW dbo.vw_DrugRegister AS
SELECT
    h.Id AS HandoverId,
    h.EncounterId,
    p.Id AS PrescriptionId,
    p.DrugName,
    p.Dosage,
    p.Frequency,
    p.Duration,
    d.BatchNumber,
    d.QuantityDispensed,
    d.ExpiryDate,
    h.HandoverAt
FROM dbo.DrugHandovers h
JOIN dbo.Dispensings d ON d.Id = h.DispensingId
JOIN dbo.Prescriptions p ON p.Id = d.PrescriptionId
WHERE h.HandoverAt IS NOT NULL;

GO
