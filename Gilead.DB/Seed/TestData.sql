SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @Now datetimeoffset = SYSDATETIMEOFFSET();
DECLARE @Today date = CAST(@Now AS date);
DECLARE @Tomorrow date = DATEADD(day, 1, @Today);
DECLARE @Yesterday date = DATEADD(day, -1, @Today);

DECLARE @RegistrarId uniqueidentifier = '10000000-0000-0000-0000-000000000001';
DECLARE @NurseId uniqueidentifier = '10000000-0000-0000-0000-000000000002';
DECLARE @DoctorId uniqueidentifier = '10000000-0000-0000-0000-000000000003';
DECLARE @PharmacistId uniqueidentifier = '10000000-0000-0000-0000-000000000004';
DECLARE @ProtocolOfficerId uniqueidentifier = '10000000-0000-0000-0000-000000000005';
DECLARE @ScientistId uniqueidentifier = '10000000-0000-0000-0000-000000000006';
DECLARE @DressingNurseId uniqueidentifier = '10000000-0000-0000-0000-000000000007';

DECLARE @ColdQueuedPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000001';
DECLARE @PharmacyPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000002';
DECLARE @LabPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000003';
DECLARE @DressingPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000004';
DECLARE @HandoverPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000005';
DECLARE @DischargedPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000006';
DECLARE @ReferredPatientId uniqueidentifier = '20000000-0000-0000-0000-000000000007';

DECLARE @ColdQueuedEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000001';
DECLARE @PharmacyEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000002';
DECLARE @LabEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000003';
DECLARE @DressingEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000004';
DECLARE @HandoverEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000005';
DECLARE @DischargedEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000006';
DECLARE @ReferredEncounterId uniqueidentifier = '30000000-0000-0000-0000-000000000007';

DECLARE @ColdQueuedVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000001';
DECLARE @PharmacyVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000002';
DECLARE @LabVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000003';
DECLARE @DressingVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000004';
DECLARE @HandoverVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000005';
DECLARE @DischargedVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000006';
DECLARE @ReferredVitalsId uniqueidentifier = '40000000-0000-0000-0000-000000000007';

DECLARE @PharmacyConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000002';
DECLARE @LabConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000003';
DECLARE @DressingConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000004';
DECLARE @HandoverConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000005';
DECLARE @DischargedConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000006';
DECLARE @ReferredConsultationId uniqueidentifier = '50000000-0000-0000-0000-000000000007';

DECLARE @PharmacyPrescriptionId uniqueidentifier = '60000000-0000-0000-0000-000000000002';
DECLARE @HandoverPrescriptionId uniqueidentifier = '60000000-0000-0000-0000-000000000005';
DECLARE @DischargedPrescriptionId uniqueidentifier = '60000000-0000-0000-0000-000000000006';

DECLARE @HandoverDispensingId uniqueidentifier = '70000000-0000-0000-0000-000000000005';
DECLARE @DischargedDispensingId uniqueidentifier = '70000000-0000-0000-0000-000000000006';

DECLARE @HandoverHandoverId uniqueidentifier = '80000000-0000-0000-0000-000000000005';
DECLARE @DischargedHandoverId uniqueidentifier = '80000000-0000-0000-0000-000000000006';

DECLARE @LabRequestId uniqueidentifier = '90000000-0000-0000-0000-000000000003';
DECLARE @DischargedLabRequestId uniqueidentifier = '90000000-0000-0000-0000-000000000006';
DECLARE @DischargedLabResultId uniqueidentifier = '91000000-0000-0000-0000-000000000006';

DECLARE @DressingOrderId uniqueidentifier = 'a0000000-0000-0000-0000-000000000004';
DECLARE @DischargedDressingOrderId uniqueidentifier = 'a0000000-0000-0000-0000-000000000006';

DECLARE @DischargedContactTraceId uniqueidentifier = 'b0000000-0000-0000-0000-000000000006';
DECLARE @ReferredContactTraceId uniqueidentifier = 'b0000000-0000-0000-0000-000000000007';
DECLARE @TodayWindowId uniqueidentifier = 'c0000000-0000-0000-0000-000000000001';

DELETE FROM dbo.ContactTraces WHERE Id IN (@DischargedContactTraceId, @ReferredContactTraceId);
DELETE FROM dbo.DrugHandovers WHERE Id IN (@HandoverHandoverId, @DischargedHandoverId);
DELETE FROM dbo.Dispensings WHERE Id IN (@HandoverDispensingId, @DischargedDispensingId);
DELETE FROM dbo.Prescriptions WHERE Id IN (@PharmacyPrescriptionId, @HandoverPrescriptionId, @DischargedPrescriptionId);
DELETE FROM dbo.LabResults WHERE Id = @DischargedLabResultId;
DELETE FROM dbo.LabRequests WHERE Id IN (@LabRequestId, @DischargedLabRequestId);
DELETE FROM dbo.DressingOrders WHERE Id IN (@DressingOrderId, @DischargedDressingOrderId);
DELETE FROM dbo.ConsultationNotes WHERE Id IN (@PharmacyConsultationId, @LabConsultationId, @DressingConsultationId, @HandoverConsultationId, @DischargedConsultationId, @ReferredConsultationId);
DELETE FROM dbo.VitalSigns WHERE Id IN (@ColdQueuedVitalsId, @PharmacyVitalsId, @LabVitalsId, @DressingVitalsId, @HandoverVitalsId, @DischargedVitalsId, @ReferredVitalsId);
DELETE FROM dbo.Encounters WHERE Id IN (@ColdQueuedEncounterId, @PharmacyEncounterId, @LabEncounterId, @DressingEncounterId, @HandoverEncounterId, @DischargedEncounterId, @ReferredEncounterId);
DELETE FROM dbo.Patients WHERE Id IN (@ColdQueuedPatientId, @PharmacyPatientId, @LabPatientId, @DressingPatientId, @HandoverPatientId, @DischargedPatientId, @ReferredPatientId);
DELETE FROM dbo.ServiceTimeWindows WHERE Id = @TodayWindowId AND [Date] <> @Today;

IF EXISTS (SELECT 1 FROM dbo.ServiceTimeWindows WHERE [Date] = @Today)
BEGIN
    UPDATE dbo.ServiceTimeWindows
    SET ColdCaseOpenTime = '00:00:00', ColdCaseCloseTime = '23:59:59'
    WHERE [Date] = @Today;
END
ELSE
BEGIN
    INSERT dbo.ServiceTimeWindows (Id, [Date], ColdCaseOpenTime, ColdCaseCloseTime, CreatedBy, CreatedAt)
    VALUES (@TodayWindowId, @Today, '00:00:00', '23:59:59', @RegistrarId, @Now);
END

INSERT dbo.Patients (Id, FullName, Age, Sex, Phone, Address, NextOfKinName, NextOfKinPhone, NextOfKinRelationship, CreatedAt)
VALUES
    (@ColdQueuedPatientId, N'Amina Yusuf', 52, N'F', N'08030000001', N'12 Church Road, Makurdi', N'Ibrahim Yusuf', N'08039000001', N'Spouse', DATEADD(hour, -6, @Now)),
    (@PharmacyPatientId, N'Chinedu Okafor', 34, N'M', N'08030000002', N'7 Mission Street, Enugu', N'Ada Okafor', N'08039000002', N'Sister', DATEADD(hour, -5, @Now)),
    (@LabPatientId, N'Grace Eze', 28, N'F', N'08030000003', N'15 Clinic Avenue, Owerri', N'Mary Eze', N'08039000003', N'Mother', DATEADD(hour, -4, @Now)),
    (@DressingPatientId, N'Musa Bello', 41, N'M', N'08030000004', N'22 Hospital Road, Kaduna', N'Halima Bello', N'08039000004', N'Wife', DATEADD(hour, -3, @Now)),
    (@HandoverPatientId, N'Blessing Nwosu', 45, N'F', N'08030000005', N'5 Unity Close, Abuja', N'Peter Nwosu', N'08039000005', N'Brother', DATEADD(hour, -2, @Now)),
    (@DischargedPatientId, N'Samuel Adeyemi', 63, N'M', N'08030000006', N'9 Market Lane, Ibadan', N'Tolu Adeyemi', N'08039000006', N'Son', DATEADD(day, -1, @Now)),
    (@ReferredPatientId, N'Fatima Abdullahi', 30, N'F', N'08030000007', N'31 Crescent Road, Kano', N'Aisha Abdullahi', N'08039000007', N'Sister', DATEADD(hour, -1, @Now));

INSERT dbo.Encounters (Id, PatientId, AdmissionType, Status, ArrivalMode, ChiefComplaint, RegisteredBy, AdmittedAt, DischargedAt, CreatedAt, UpdatedAt)
VALUES
    (@ColdQueuedEncounterId, @ColdQueuedPatientId, N'ColdCase', N'Queued', N'WalkedIn', N'Routine hypertension follow-up and medication review.', @RegistrarId, DATEADD(hour, -5, @Now), NULL, DATEADD(hour, -5, @Now), DATEADD(hour, -5, @Now)),
    (@PharmacyEncounterId, @PharmacyPatientId, N'ColdCase', N'PharmacyPending', N'WalkedIn', N'Fever, malaise, and headache for two days.', @RegistrarId, DATEADD(hour, -4, @Now), NULL, DATEADD(hour, -4, @Now), DATEADD(hour, -3, @Now)),
    (@LabEncounterId, @LabPatientId, N'ColdCase', N'LabPending', N'Supported', N'Weakness and dizziness with suspected anemia.', @RegistrarId, DATEADD(hour, -4, @Now), NULL, DATEADD(hour, -4, @Now), DATEADD(hour, -3, @Now)),
    (@DressingEncounterId, @DressingPatientId, N'ColdCase', N'DressingPending', N'WalkedIn', N'Dressing review for lower-leg wound.', @RegistrarId, DATEADD(hour, -3, @Now), NULL, DATEADD(hour, -3, @Now), DATEADD(hour, -2, @Now)),
    (@HandoverEncounterId, @HandoverPatientId, N'ColdCase', N'AwaitingHandover', N'WalkedIn', N'Productive cough and chest discomfort.', @RegistrarId, DATEADD(hour, -2, @Now), NULL, DATEADD(hour, -2, @Now), DATEADD(hour, -1, @Now)),
    (@DischargedEncounterId, @DischargedPatientId, N'ColdCase', N'Discharged', N'WalkedIn', N'Diabetes follow-up with foot-care review.', @RegistrarId, DATEADD(day, -1, @Now), DATEADD(hour, -1, @Now), DATEADD(day, -1, @Now), DATEADD(hour, -1, @Now)),
    (@ReferredEncounterId, @ReferredPatientId, N'Emergency', N'Referred', N'Stretcher', N'Acute abdominal pain requiring surgical evaluation.', @RegistrarId, DATEADD(hour, -2, @Now), DATEADD(minute, -20, @Now), DATEADD(hour, -2, @Now), DATEADD(minute, -20, @Now));

INSERT dbo.VitalSigns (Id, EncounterId, RecordedBy, BloodPressureSystolic, BloodPressureDiastolic, PulseRate, Temperature, Spo2, RespiratoryRate, Weight, Notes, RecordedAt)
VALUES
    (@ColdQueuedVitalsId, @ColdQueuedEncounterId, @NurseId, 148, 92, 84, 36.80, 98, 18, 76.50, N'Elevated BP; queued for consultation.', DATEADD(hour, -5, @Now)),
    (@PharmacyVitalsId, @PharmacyEncounterId, @NurseId, 118, 74, 96, 38.10, 97, 20, 68.20, N'Febrile but stable.', DATEADD(hour, -4, @Now)),
    (@LabVitalsId, @LabEncounterId, @NurseId, 104, 68, 102, 36.70, 99, 18, 58.40, N'Mild tachycardia.', DATEADD(hour, -4, @Now)),
    (@DressingVitalsId, @DressingEncounterId, @NurseId, 126, 82, 78, 36.60, 98, 16, 82.00, N'Vitals stable before dressing.', DATEADD(hour, -3, @Now)),
    (@HandoverVitalsId, @HandoverEncounterId, @NurseId, 122, 78, 88, 37.40, 96, 19, 71.10, N'Chest clear with occasional cough.', DATEADD(hour, -2, @Now)),
    (@DischargedVitalsId, @DischargedEncounterId, @NurseId, 132, 84, 80, 36.50, 98, 17, 79.80, N'No acute distress.', DATEADD(day, -1, @Now)),
    (@ReferredVitalsId, @ReferredEncounterId, @NurseId, 96, 62, 118, 37.90, 95, 24, 61.70, N'Pain score 8/10; urgent referral initiated.', DATEADD(hour, -2, @Now));

INSERT dbo.ConsultationNotes (Id, EncounterId, DoctorId, Diagnosis, ClinicalNotes, RequiresLab, RequiresDressing, IsReferral, ReferralFacility, ReferralReason, ConsultedAt)
VALUES
    (@PharmacyConsultationId, @PharmacyEncounterId, @DoctorId, N'["Uncomplicated malaria"]', N'Positive clinical features for malaria. Start oral antimalarial and analgesic.', 0, 0, 0, NULL, NULL, DATEADD(hour, -3, @Now)),
    (@LabConsultationId, @LabEncounterId, @DoctorId, N'["Suspected anemia"]', N'Patient reports fatigue and dizziness. Request full blood count before treatment decision.', 1, 0, 0, NULL, NULL, DATEADD(hour, -3, @Now)),
    (@DressingConsultationId, @DressingEncounterId, @DoctorId, N'["Clean granulating leg wound"]', N'Wound reviewed. Requires saline cleaning and sterile dressing.', 0, 1, 0, NULL, NULL, DATEADD(hour, -2, @Now)),
    (@HandoverConsultationId, @HandoverEncounterId, @DoctorId, N'["Acute bronchitis"]', N'No danger signs. Dispense antibiotics and counsel on adherence.', 0, 0, 0, NULL, NULL, DATEADD(hour, -2, @Now)),
    (@DischargedConsultationId, @DischargedEncounterId, @DoctorId, N'["Type 2 diabetes mellitus","Foot-care review"]', N'Glycemic control acceptable. Completed pharmacy handover and counselling.', 1, 1, 0, NULL, NULL, DATEADD(day, -1, @Now)),
    (@ReferredConsultationId, @ReferredEncounterId, @DoctorId, N'["Acute abdomen"]', N'Guarding and rebound tenderness present. Stabilized and referred for surgical evaluation.', 0, 0, 1, N'St. Raphael Specialist Hospital', N'Requires urgent abdominal imaging and surgical review.', DATEADD(hour, -1, @Now));

INSERT dbo.Prescriptions (Id, ConsultationNoteId, EncounterId, DrugName, Dosage, Frequency, Duration, Route, Instructions, Status, IssuedAt)
VALUES
    (@PharmacyPrescriptionId, @PharmacyConsultationId, @PharmacyEncounterId, N'Artemether/Lumefantrine', N'80/480 mg', N'Twice daily', N'3 days', N'Oral', N'Take after meals.', N'Pending', DATEADD(hour, -3, @Now)),
    (@HandoverPrescriptionId, @HandoverConsultationId, @HandoverEncounterId, N'Amoxicillin/Clavulanate', N'625 mg', N'Twice daily', N'5 days', N'Oral', N'Complete full course.', N'Dispensed', DATEADD(hour, -2, @Now)),
    (@DischargedPrescriptionId, @DischargedConsultationId, @DischargedEncounterId, N'Metformin', N'500 mg', N'Twice daily', N'30 days', N'Oral', N'Take with food.', N'HandedOver', DATEADD(day, -1, @Now));

INSERT dbo.Dispensings (Id, PrescriptionId, PharmacistId, DrugName, QuantityDispensed, BatchNumber, ExpiryDate, Notes, DispensedAt)
VALUES
    (@HandoverDispensingId, @HandoverPrescriptionId, @PharmacistId, N'Amoxicillin/Clavulanate', 10, N'AMX-TEST-001', @Tomorrow, N'Packed and awaiting protocol handover.', DATEADD(hour, -1, @Now)),
    (@DischargedDispensingId, @DischargedPrescriptionId, @PharmacistId, N'Metformin', 60, N'MET-TEST-001', DATEADD(month, 18, @Today), N'One-month refill dispensed.', DATEADD(hour, -2, @Now));

INSERT dbo.DrugHandovers (Id, DispensingId, EncounterId, ProtocolOfficerId, PatientNameVerified, DrugListVerified, DosageCounsellingDone, DurationCounsellingDone, CounsellingNotes, HandoverAt)
VALUES
    (@HandoverHandoverId, @HandoverDispensingId, @HandoverEncounterId, NULL, 0, 0, 0, 0, NULL, NULL),
    (@DischargedHandoverId, @DischargedDispensingId, @DischargedEncounterId, @ProtocolOfficerId, 1, 1, 1, 1, N'Patient understood dose timing and follow-up date.', DATEADD(hour, -1, @Now));

INSERT dbo.LabRequests (Id, ConsultationNoteId, EncounterId, TestName, ClinicalIndication, Status, RequestedAt)
VALUES
    (@LabRequestId, @LabConsultationId, @LabEncounterId, N'Full Blood Count', N'Assess suspected anemia.', N'Pending', DATEADD(hour, -3, @Now)),
    (@DischargedLabRequestId, @DischargedConsultationId, @DischargedEncounterId, N'Fasting Blood Glucose', N'Diabetes follow-up.', N'Completed', DATEADD(day, -1, @Now));

INSERT dbo.LabResults (Id, LabRequestId, ScientistId, TestName, Findings, Conclusion, [Values], CompletedAt)
VALUES
    (@DischargedLabResultId, @DischargedLabRequestId, @ScientistId, N'Fasting Blood Glucose', N'Fasting glucose mildly elevated.', N'Review medication adherence and continue current plan.', N'[{"Parameter":"Glucose","Value":"138","Unit":"mg/dL","ReferenceRange":"70-99"}]', DATEADD(hour, -3, @Now));

INSERT dbo.DressingOrders (Id, ConsultationNoteId, EncounterId, Instructions, Status, PerformedBy, ProcedureNotes, CompletedAt, CreatedAt)
VALUES
    (@DressingOrderId, @DressingConsultationId, @DressingEncounterId, N'Clean wound with normal saline, apply povidone iodine, and cover with sterile gauze.', N'Pending', NULL, NULL, NULL, DATEADD(hour, -2, @Now)),
    (@DischargedDressingOrderId, @DischargedConsultationId, @DischargedEncounterId, N'Inspect foot, clean minor abrasion, and apply protective dressing.', N'Completed', @DressingNurseId, N'No discharge or signs of infection. Dressing completed.', DATEADD(hour, -2, @Now), DATEADD(day, -1, @Now));

INSERT dbo.ContactTraces (Id, EncounterId, RecordedBy, NextOfKinName, NextOfKinPhone, NextOfKinRelationship, ResidentialAddress, WorkplaceAddress, DischargeNotes, ReferralDestination, RecordedAt)
VALUES
    (@DischargedContactTraceId, @DischargedEncounterId, @ProtocolOfficerId, N'Tolu Adeyemi', N'08039000006', N'Son', N'9 Market Lane, Ibadan', N'Adeyemi Stores, Dugbe Market', N'Discharged with diabetes care advice and outpatient follow-up.', NULL, DATEADD(hour, -1, @Now)),
    (@ReferredContactTraceId, @ReferredEncounterId, @ProtocolOfficerId, N'Aisha Abdullahi', N'08039000007', N'Sister', N'31 Crescent Road, Kano', N'Kano Textile Market', N'Referred after emergency stabilization.', N'St. Raphael Specialist Hospital', DATEADD(minute, -20, @Now));

COMMIT TRANSACTION;

SELECT
    (SELECT COUNT(*) FROM dbo.Patients WHERE Id IN (@ColdQueuedPatientId, @PharmacyPatientId, @LabPatientId, @DressingPatientId, @HandoverPatientId, @DischargedPatientId, @ReferredPatientId)) AS SeedPatients,
    (SELECT COUNT(*) FROM dbo.Encounters WHERE Id IN (@ColdQueuedEncounterId, @PharmacyEncounterId, @LabEncounterId, @DressingEncounterId, @HandoverEncounterId, @DischargedEncounterId, @ReferredEncounterId)) AS SeedEncounters,
    (SELECT COUNT(*) FROM dbo.Prescriptions WHERE Id IN (@PharmacyPrescriptionId, @HandoverPrescriptionId, @DischargedPrescriptionId)) AS SeedPrescriptions,
    (SELECT COUNT(*) FROM dbo.LabRequests WHERE Id IN (@LabRequestId, @DischargedLabRequestId)) AS SeedLabRequests,
    (SELECT COUNT(*) FROM dbo.DressingOrders WHERE Id IN (@DressingOrderId, @DischargedDressingOrderId)) AS SeedDressingOrders,
    (SELECT COUNT(*) FROM dbo.DrugHandovers WHERE Id IN (@HandoverHandoverId, @DischargedHandoverId)) AS SeedHandovers;
