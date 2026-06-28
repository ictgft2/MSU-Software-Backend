CREATE TYPE dbo.PrescriptionTvp AS TABLE (
    Id uniqueidentifier NOT NULL,
    ConsultationNoteId uniqueidentifier NOT NULL,
    EncounterId uniqueidentifier NOT NULL,
    DrugName nvarchar(200) NOT NULL,
    Dosage nvarchar(100) NOT NULL,
    Frequency nvarchar(100) NOT NULL,
    Duration nvarchar(100) NOT NULL,
    Route nvarchar(30) NOT NULL,
    Instructions nvarchar(1000) NULL,
    Status nvarchar(30) NOT NULL,
    IssuedAt datetimeoffset NOT NULL
);

CREATE TYPE dbo.LabRequestTvp AS TABLE (
    Id uniqueidentifier NOT NULL,
    ConsultationNoteId uniqueidentifier NOT NULL,
    EncounterId uniqueidentifier NOT NULL,
    TestName nvarchar(200) NOT NULL,
    ClinicalIndication nvarchar(1000) NOT NULL,
    Status nvarchar(30) NOT NULL,
    RequestedAt datetimeoffset NOT NULL
);
