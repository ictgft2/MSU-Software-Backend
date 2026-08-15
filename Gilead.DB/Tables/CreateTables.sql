CREATE TABLE public.Patients (
    Id uuid NOT NULL,
    FullName varchar(200) NOT NULL,
    Age integer NOT NULL,
    Sex varchar(1) NOT NULL,
    Phone varchar(40) NOT NULL,
    Address varchar(500) NOT NULL,
    NextOfKinName varchar(200) NOT NULL,
    NextOfKinPhone varchar(40) NOT NULL,
    NextOfKinRelationship varchar(100) NOT NULL,
    CreatedAt timestamptz NOT NULL,
    CONSTRAINT pk_patients PRIMARY KEY (Id)
);

CREATE TABLE public.Encounters (
    Id uuid NOT NULL,
    PatientId uuid NOT NULL,
    AdmissionType varchar(30) NOT NULL,
    Status varchar(40) NOT NULL,
    ArrivalMode varchar(30) NOT NULL,
    ChiefComplaint varchar(1000) NOT NULL,
    RegisteredBy uuid NOT NULL,
    AdmittedAt timestamptz NOT NULL,
    DischargedAt timestamptz NULL,
    CreatedAt timestamptz NOT NULL,
    UpdatedAt timestamptz NOT NULL,
    CONSTRAINT pk_encounters PRIMARY KEY (Id),
    CONSTRAINT fk_encounters_patients FOREIGN KEY (PatientId) REFERENCES public.Patients (Id)
);

CREATE TABLE public.VitalSigns (
    Id uuid NOT NULL,
    EncounterId uuid NOT NULL,
    RecordedBy uuid NOT NULL,
    BloodPressureSystolic integer NULL,
    BloodPressureDiastolic integer NULL,
    PulseRate integer NULL,
    Temperature numeric(5,2) NULL,
    Spo2 integer NULL,
    RespiratoryRate integer NULL,
    Weight numeric(8,2) NULL,
    Notes varchar(1000) NULL,
    RecordedAt timestamptz NOT NULL,
    CONSTRAINT pk_vitalsigns PRIMARY KEY (Id),
    CONSTRAINT fk_vitalsigns_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.ConsultationNotes (
    Id uuid NOT NULL,
    EncounterId uuid NOT NULL,
    DoctorId uuid NOT NULL,
    Diagnosis text NOT NULL,
    ClinicalNotes text NOT NULL,
    RequiresLab boolean NOT NULL,
    RequiresDressing boolean NOT NULL,
    IsReferral boolean NOT NULL,
    ReferralFacility varchar(250) NULL,
    ReferralReason varchar(1000) NULL,
    ConsultedAt timestamptz NOT NULL,
    CONSTRAINT pk_consultationnotes PRIMARY KEY (Id),
    CONSTRAINT fk_consultationnotes_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.Prescriptions (
    Id uuid NOT NULL,
    ConsultationNoteId uuid NOT NULL,
    EncounterId uuid NOT NULL,
    DrugName varchar(200) NOT NULL,
    Dosage varchar(100) NOT NULL,
    Frequency varchar(100) NOT NULL,
    Duration varchar(100) NOT NULL,
    Route varchar(30) NOT NULL,
    Instructions varchar(1000) NULL,
    Status varchar(30) NOT NULL,
    IssuedAt timestamptz NOT NULL,
    CONSTRAINT pk_prescriptions PRIMARY KEY (Id),
    CONSTRAINT fk_prescriptions_consultations FOREIGN KEY (ConsultationNoteId) REFERENCES public.ConsultationNotes (Id),
    CONSTRAINT fk_prescriptions_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.Dispensings (
    Id uuid NOT NULL,
    PrescriptionId uuid NOT NULL,
    PharmacistId uuid NOT NULL,
    DrugName varchar(200) NOT NULL,
    QuantityDispensed integer NOT NULL,
    BatchNumber varchar(100) NOT NULL,
    ExpiryDate date NOT NULL,
    Notes varchar(1000) NULL,
    DispensedAt timestamptz NOT NULL,
    CONSTRAINT pk_dispensings PRIMARY KEY (Id),
    CONSTRAINT fk_dispensings_prescriptions FOREIGN KEY (PrescriptionId) REFERENCES public.Prescriptions (Id)
);

CREATE TABLE public.DrugHandovers (
    Id uuid NOT NULL,
    DispensingId uuid NOT NULL,
    EncounterId uuid NOT NULL,
    ProtocolOfficerId uuid NULL,
    PatientNameVerified boolean NOT NULL DEFAULT false,
    DrugListVerified boolean NOT NULL DEFAULT false,
    DosageCounsellingDone boolean NOT NULL DEFAULT false,
    DurationCounsellingDone boolean NOT NULL DEFAULT false,
    CounsellingNotes varchar(1000) NULL,
    HandoverAt timestamptz NULL,
    CONSTRAINT pk_drughandovers PRIMARY KEY (Id),
    CONSTRAINT fk_drughandovers_dispensings FOREIGN KEY (DispensingId) REFERENCES public.Dispensings (Id),
    CONSTRAINT fk_drughandovers_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.LabRequests (
    Id uuid NOT NULL,
    ConsultationNoteId uuid NOT NULL,
    EncounterId uuid NOT NULL,
    TestName varchar(200) NOT NULL,
    ClinicalIndication varchar(1000) NOT NULL,
    Status varchar(30) NOT NULL,
    RequestedAt timestamptz NOT NULL,
    CONSTRAINT pk_labrequests PRIMARY KEY (Id),
    CONSTRAINT fk_labrequests_consultations FOREIGN KEY (ConsultationNoteId) REFERENCES public.ConsultationNotes (Id),
    CONSTRAINT fk_labrequests_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.LabResults (
    Id uuid NOT NULL,
    LabRequestId uuid NOT NULL,
    ScientistId uuid NOT NULL,
    TestName varchar(200) NOT NULL,
    Findings text NOT NULL,
    Conclusion text NOT NULL,
    "values" text NOT NULL,
    CompletedAt timestamptz NOT NULL,
    CONSTRAINT pk_labresults PRIMARY KEY (Id),
    CONSTRAINT fk_labresults_labrequests FOREIGN KEY (LabRequestId) REFERENCES public.LabRequests (Id)
);

CREATE TABLE public.DressingOrders (
    Id uuid NOT NULL,
    ConsultationNoteId uuid NOT NULL,
    EncounterId uuid NOT NULL,
    Instructions text NOT NULL,
    Status varchar(30) NOT NULL,
    PerformedBy uuid NULL,
    ProcedureNotes text NULL,
    CompletedAt timestamptz NULL,
    CreatedAt timestamptz NOT NULL,
    CONSTRAINT pk_dressingorders PRIMARY KEY (Id),
    CONSTRAINT fk_dressingorders_consultations FOREIGN KEY (ConsultationNoteId) REFERENCES public.ConsultationNotes (Id),
    CONSTRAINT fk_dressingorders_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.ContactTraces (
    Id uuid NOT NULL,
    EncounterId uuid NOT NULL,
    RecordedBy uuid NOT NULL,
    NextOfKinName varchar(200) NOT NULL,
    NextOfKinPhone varchar(40) NOT NULL,
    NextOfKinRelationship varchar(100) NOT NULL,
    ResidentialAddress varchar(500) NOT NULL,
    WorkplaceAddress varchar(500) NOT NULL,
    DischargeNotes text NOT NULL,
    ReferralDestination varchar(250) NULL,
    RecordedAt timestamptz NOT NULL,
    CONSTRAINT pk_contacttraces PRIMARY KEY (Id),
    CONSTRAINT uq_contacttraces_encounter UNIQUE (EncounterId),
    CONSTRAINT fk_contacttraces_encounters FOREIGN KEY (EncounterId) REFERENCES public.Encounters (Id)
);

CREATE TABLE public.ServiceTimeWindows (
    Id uuid NOT NULL,
    Date date NOT NULL,
    ColdCaseOpenTime time NOT NULL,
    ColdCaseCloseTime time NOT NULL,
    CreatedBy uuid NOT NULL,
    CreatedAt timestamptz NOT NULL,
    CONSTRAINT pk_servicetimewindows PRIMARY KEY (Id),
    CONSTRAINT uq_servicetimewindows_date UNIQUE (Date)
);

CREATE INDEX ix_encounters_patientid ON public.Encounters (PatientId);
CREATE INDEX ix_encounters_status_createdat ON public.Encounters (Status, CreatedAt DESC);
CREATE INDEX ix_vitalsigns_encounter_recordedat ON public.VitalSigns (EncounterId, RecordedAt DESC);
CREATE INDEX ix_consultationnotes_encounterid ON public.ConsultationNotes (EncounterId);
CREATE INDEX ix_prescriptions_consultationid ON public.Prescriptions (ConsultationNoteId);
CREATE INDEX ix_prescriptions_encounter_status_issuedat ON public.Prescriptions (EncounterId, Status, IssuedAt);
CREATE INDEX ix_dispensings_prescriptionid ON public.Dispensings (PrescriptionId);
CREATE INDEX ix_drughandovers_dispensingid ON public.DrugHandovers (DispensingId);
CREATE INDEX ix_drughandovers_encounterid ON public.DrugHandovers (EncounterId);
CREATE INDEX ix_drughandovers_pending ON public.DrugHandovers (Id) WHERE HandoverAt IS NULL;
CREATE INDEX ix_labrequests_consultationid ON public.LabRequests (ConsultationNoteId);
CREATE INDEX ix_labrequests_encounter_status_requestedat ON public.LabRequests (EncounterId, Status, RequestedAt);
CREATE INDEX ix_labresults_request_completedat ON public.LabResults (LabRequestId, CompletedAt DESC);
CREATE INDEX ix_dressingorders_consultationid ON public.DressingOrders (ConsultationNoteId);
CREATE INDEX ix_dressingorders_encounter_status_createdat ON public.DressingOrders (EncounterId, Status, CreatedAt);

CREATE VIEW public.vw_DrugRegister AS
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
FROM public.DrugHandovers h
JOIN public.Dispensings d ON d.Id = h.DispensingId
JOIN public.Prescriptions p ON p.Id = d.PrescriptionId
WHERE h.HandoverAt IS NOT NULL;
