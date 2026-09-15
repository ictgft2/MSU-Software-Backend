BEGIN;

DO $seed$
DECLARE
    seed_now timestamptz := CURRENT_TIMESTAMP;
    today date := (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')::date;
    tomorrow date := (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')::date + 1;

    registrar_id uuid := '10000000-0000-0000-0000-000000000001';
    nurse_id uuid := '10000000-0000-0000-0000-000000000002';
    doctor_id uuid := '10000000-0000-0000-0000-000000000003';
    pharmacist_id uuid := '10000000-0000-0000-0000-000000000004';
    protocol_officer_id uuid := '10000000-0000-0000-0000-000000000005';
    scientist_id uuid := '10000000-0000-0000-0000-000000000006';
    dressing_nurse_id uuid := '10000000-0000-0000-0000-000000000007';

    cold_queued_patient_id uuid := '20000000-0000-0000-0000-000000000001';
    pharmacy_patient_id uuid := '20000000-0000-0000-0000-000000000002';
    lab_patient_id uuid := '20000000-0000-0000-0000-000000000003';
    dressing_patient_id uuid := '20000000-0000-0000-0000-000000000004';
    handover_patient_id uuid := '20000000-0000-0000-0000-000000000005';
    discharged_patient_id uuid := '20000000-0000-0000-0000-000000000006';
    referred_patient_id uuid := '20000000-0000-0000-0000-000000000007';

    cold_queued_encounter_id uuid := '30000000-0000-0000-0000-000000000001';
    pharmacy_encounter_id uuid := '30000000-0000-0000-0000-000000000002';
    lab_encounter_id uuid := '30000000-0000-0000-0000-000000000003';
    dressing_encounter_id uuid := '30000000-0000-0000-0000-000000000004';
    handover_encounter_id uuid := '30000000-0000-0000-0000-000000000005';
    discharged_encounter_id uuid := '30000000-0000-0000-0000-000000000006';
    referred_encounter_id uuid := '30000000-0000-0000-0000-000000000007';

    cold_queued_vitals_id uuid := '40000000-0000-0000-0000-000000000001';
    pharmacy_vitals_id uuid := '40000000-0000-0000-0000-000000000002';
    lab_vitals_id uuid := '40000000-0000-0000-0000-000000000003';
    dressing_vitals_id uuid := '40000000-0000-0000-0000-000000000004';
    handover_vitals_id uuid := '40000000-0000-0000-0000-000000000005';
    discharged_vitals_id uuid := '40000000-0000-0000-0000-000000000006';
    referred_vitals_id uuid := '40000000-0000-0000-0000-000000000007';

    pharmacy_consultation_id uuid := '50000000-0000-0000-0000-000000000002';
    lab_consultation_id uuid := '50000000-0000-0000-0000-000000000003';
    dressing_consultation_id uuid := '50000000-0000-0000-0000-000000000004';
    handover_consultation_id uuid := '50000000-0000-0000-0000-000000000005';
    discharged_consultation_id uuid := '50000000-0000-0000-0000-000000000006';
    referred_consultation_id uuid := '50000000-0000-0000-0000-000000000007';

    pharmacy_prescription_id uuid := '60000000-0000-0000-0000-000000000002';
    handover_prescription_id uuid := '60000000-0000-0000-0000-000000000005';
    discharged_prescription_id uuid := '60000000-0000-0000-0000-000000000006';

    handover_dispensing_id uuid := '70000000-0000-0000-0000-000000000005';
    discharged_dispensing_id uuid := '70000000-0000-0000-0000-000000000006';
    handover_handover_id uuid := '80000000-0000-0000-0000-000000000005';
    discharged_handover_id uuid := '80000000-0000-0000-0000-000000000006';
    lab_request_id uuid := '90000000-0000-0000-0000-000000000003';
    discharged_lab_request_id uuid := '90000000-0000-0000-0000-000000000006';
    discharged_lab_result_id uuid := '91000000-0000-0000-0000-000000000006';
    dressing_order_id uuid := 'a0000000-0000-0000-0000-000000000004';
    discharged_dressing_order_id uuid := 'a0000000-0000-0000-0000-000000000006';
    discharged_contact_trace_id uuid := 'b0000000-0000-0000-0000-000000000006';
    referred_contact_trace_id uuid := 'b0000000-0000-0000-0000-000000000007';
    today_window_id uuid := 'c0000000-0000-0000-0000-000000000001';
BEGIN
    DELETE FROM public.ContactTraces WHERE Id IN (discharged_contact_trace_id, referred_contact_trace_id);
    DELETE FROM public.DrugHandovers WHERE Id IN (handover_handover_id, discharged_handover_id);
    DELETE FROM public.Dispensings WHERE Id IN (handover_dispensing_id, discharged_dispensing_id);
    DELETE FROM public.Prescriptions WHERE Id IN (pharmacy_prescription_id, handover_prescription_id, discharged_prescription_id);
    DELETE FROM public.LabResults WHERE Id = discharged_lab_result_id;
    DELETE FROM public.LabRequests WHERE Id IN (lab_request_id, discharged_lab_request_id);
    DELETE FROM public.DressingOrders WHERE Id IN (dressing_order_id, discharged_dressing_order_id);
    DELETE FROM public.ConsultationNotes WHERE Id IN (pharmacy_consultation_id, lab_consultation_id, dressing_consultation_id, handover_consultation_id, discharged_consultation_id, referred_consultation_id);
    DELETE FROM public.VitalSigns WHERE Id IN (cold_queued_vitals_id, pharmacy_vitals_id, lab_vitals_id, dressing_vitals_id, handover_vitals_id, discharged_vitals_id, referred_vitals_id);
    DELETE FROM public.Encounters WHERE Id IN (cold_queued_encounter_id, pharmacy_encounter_id, lab_encounter_id, dressing_encounter_id, handover_encounter_id, discharged_encounter_id, referred_encounter_id);
    DELETE FROM public.Patients WHERE Id IN (cold_queued_patient_id, pharmacy_patient_id, lab_patient_id, dressing_patient_id, handover_patient_id, discharged_patient_id, referred_patient_id);
    DELETE FROM public.Staff WHERE Id IN (registrar_id, nurse_id, doctor_id, pharmacist_id, protocol_officer_id, scientist_id, dressing_nurse_id);
    DELETE FROM public.ServiceTimeWindows WHERE Id = today_window_id AND Date <> today;

    INSERT INTO public.Staff (Id, FullName, Email, Role, IsActive, PasswordHash, CreatedAt)
    VALUES
        (registrar_id, 'Adebayo Okafor', 'registrar@gilead.test', 'Registrar', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (nurse_id, 'Ngozi Eze', 'nurse@gilead.test', 'Nurse', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (doctor_id, 'Jane Adeyemi', 'doctor@gilead.test', 'Doctor', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (pharmacist_id, 'Samuel Bello', 'pharmacist@gilead.test', 'Pharmacist', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (protocol_officer_id, 'Ibrahim Yusuf', 'protocol@gilead.test', 'ProtocolOfficer', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (scientist_id, 'Grace Mensah', 'scientist@gilead.test', 'Scientist', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now),
        (dressing_nurse_id, 'Musa Ibrahim', 'dressing.nurse@gilead.test', 'DressingNurse', true, 'v1$120000$AQEBAQEBAQEBAQEBAQEBAQ==$hrC/CDDZbqVdgvyM4nWybJqfi5rAxkHSCyE2IwW0DHI=', seed_now);

    IF EXISTS (SELECT 1 FROM public.ServiceTimeWindows WHERE Date = today) THEN
        UPDATE public.ServiceTimeWindows
        SET ColdCaseOpenTime = '00:00:00', ColdCaseCloseTime = '23:59:59'
        WHERE Date = today;
    ELSE
        INSERT INTO public.ServiceTimeWindows (Id, Date, ColdCaseOpenTime, ColdCaseCloseTime, CreatedBy, CreatedAt)
        VALUES (today_window_id, today, '00:00:00', '23:59:59', registrar_id, seed_now);
    END IF;

    INSERT INTO public.Patients (Id, FullName, Age, Sex, Phone, Address, NextOfKinName, NextOfKinPhone, NextOfKinRelationship, CreatedAt)
    VALUES
        (cold_queued_patient_id, 'Amina Yusuf', 52, 'F', '08030000001', '12 Church Road, Makurdi', 'Ibrahim Yusuf', '08039000001', 'Spouse', seed_now - interval '6 hours'),
        (pharmacy_patient_id, 'Chinedu Okafor', 34, 'M', '08030000002', '7 Mission Street, Enugu', 'Ada Okafor', '08039000002', 'Sister', seed_now - interval '5 hours'),
        (lab_patient_id, 'Grace Eze', 28, 'F', '08030000003', '15 Clinic Avenue, Owerri', 'Mary Eze', '08039000003', 'Mother', seed_now - interval '4 hours'),
        (dressing_patient_id, 'Musa Bello', 41, 'M', '08030000004', '22 Hospital Road, Kaduna', 'Halima Bello', '08039000004', 'Wife', seed_now - interval '3 hours'),
        (handover_patient_id, 'Blessing Nwosu', 45, 'F', '08030000005', '5 Unity Close, Abuja', 'Peter Nwosu', '08039000005', 'Brother', seed_now - interval '2 hours'),
        (discharged_patient_id, 'Samuel Adeyemi', 63, 'M', '08030000006', '9 Market Lane, Ibadan', 'Tolu Adeyemi', '08039000006', 'Son', seed_now - interval '1 day'),
        (referred_patient_id, 'Fatima Abdullahi', 30, 'F', '08030000007', '31 Crescent Road, Kano', 'Aisha Abdullahi', '08039000007', 'Sister', seed_now - interval '1 hour');

    INSERT INTO public.Encounters (Id, PatientId, AdmissionType, Status, ArrivalMode, ChiefComplaint, RegisteredBy, AdmittedAt, DischargedAt, CreatedAt, UpdatedAt)
    VALUES
        (cold_queued_encounter_id, cold_queued_patient_id, 'ColdCase', 'Queued', 'WalkedIn', 'Routine hypertension follow-up and medication review.', registrar_id, seed_now - interval '5 hours', NULL, seed_now - interval '5 hours', seed_now - interval '5 hours'),
        (pharmacy_encounter_id, pharmacy_patient_id, 'ColdCase', 'PharmacyPending', 'WalkedIn', 'Fever, malaise, and headache for two days.', registrar_id, seed_now - interval '4 hours', NULL, seed_now - interval '4 hours', seed_now - interval '3 hours'),
        (lab_encounter_id, lab_patient_id, 'ColdCase', 'LabPending', 'Supported', 'Weakness and dizziness with suspected anemia.', registrar_id, seed_now - interval '4 hours', NULL, seed_now - interval '4 hours', seed_now - interval '3 hours'),
        (dressing_encounter_id, dressing_patient_id, 'ColdCase', 'DressingPending', 'WalkedIn', 'Dressing review for lower-leg wound.', registrar_id, seed_now - interval '3 hours', NULL, seed_now - interval '3 hours', seed_now - interval '2 hours'),
        (handover_encounter_id, handover_patient_id, 'ColdCase', 'AwaitingHandover', 'WalkedIn', 'Productive cough and chest discomfort.', registrar_id, seed_now - interval '2 hours', NULL, seed_now - interval '2 hours', seed_now - interval '1 hour'),
        (discharged_encounter_id, discharged_patient_id, 'ColdCase', 'Discharged', 'WalkedIn', 'Diabetes follow-up with foot-care review.', registrar_id, seed_now - interval '1 day', seed_now - interval '1 hour', seed_now - interval '1 day', seed_now - interval '1 hour'),
        (referred_encounter_id, referred_patient_id, 'Emergency', 'Referred', 'Stretcher', 'Acute abdominal pain requiring surgical evaluation.', registrar_id, seed_now - interval '2 hours', seed_now - interval '20 minutes', seed_now - interval '2 hours', seed_now - interval '20 minutes');

    INSERT INTO public.VitalSigns (Id, EncounterId, RecordedBy, BloodPressureSystolic, BloodPressureDiastolic, PulseRate, Temperature, Spo2, RespiratoryRate, Weight, Notes, RecordedAt)
    VALUES
        (cold_queued_vitals_id, cold_queued_encounter_id, nurse_id, 148, 92, 84, 36.80, 98, 18, 76.50, 'Elevated BP; queued for consultation.', seed_now - interval '5 hours'),
        (pharmacy_vitals_id, pharmacy_encounter_id, nurse_id, 118, 74, 96, 38.10, 97, 20, 68.20, 'Febrile but stable.', seed_now - interval '4 hours'),
        (lab_vitals_id, lab_encounter_id, nurse_id, 104, 68, 102, 36.70, 99, 18, 58.40, 'Mild tachycardia.', seed_now - interval '4 hours'),
        (dressing_vitals_id, dressing_encounter_id, nurse_id, 126, 82, 78, 36.60, 98, 16, 82.00, 'Vitals stable before dressing.', seed_now - interval '3 hours'),
        (handover_vitals_id, handover_encounter_id, nurse_id, 122, 78, 88, 37.40, 96, 19, 71.10, 'Chest clear with occasional cough.', seed_now - interval '2 hours'),
        (discharged_vitals_id, discharged_encounter_id, nurse_id, 132, 84, 80, 36.50, 98, 17, 79.80, 'No acute distress.', seed_now - interval '1 day'),
        (referred_vitals_id, referred_encounter_id, nurse_id, 96, 62, 118, 37.90, 95, 24, 61.70, 'Pain score 8/10; urgent referral initiated.', seed_now - interval '2 hours');

    INSERT INTO public.ConsultationNotes (Id, EncounterId, DoctorId, Diagnosis, ClinicalNotes, RequiresLab, RequiresDressing, IsReferral, ReferralFacility, ReferralReason, ConsultedAt)
    VALUES
        (pharmacy_consultation_id, pharmacy_encounter_id, doctor_id, '["Uncomplicated malaria"]', 'Positive clinical features for malaria. Start oral antimalarial and analgesic.', false, false, false, NULL, NULL, seed_now - interval '3 hours'),
        (lab_consultation_id, lab_encounter_id, doctor_id, '["Suspected anemia"]', 'Patient reports fatigue and dizziness. Request full blood count before treatment decision.', true, false, false, NULL, NULL, seed_now - interval '3 hours'),
        (dressing_consultation_id, dressing_encounter_id, doctor_id, '["Clean granulating leg wound"]', 'Wound reviewed. Requires saline cleaning and sterile dressing.', false, true, false, NULL, NULL, seed_now - interval '2 hours'),
        (handover_consultation_id, handover_encounter_id, doctor_id, '["Acute bronchitis"]', 'No danger signs. Dispense antibiotics and counsel on adherence.', false, false, false, NULL, NULL, seed_now - interval '2 hours'),
        (discharged_consultation_id, discharged_encounter_id, doctor_id, '["Type 2 diabetes mellitus","Foot-care review"]', 'Glycemic control acceptable. Completed pharmacy handover and counselling.', true, true, false, NULL, NULL, seed_now - interval '1 day'),
        (referred_consultation_id, referred_encounter_id, doctor_id, '["Acute abdomen"]', 'Guarding and rebound tenderness present. Stabilized and referred for surgical evaluation.', false, false, true, 'St. Raphael Specialist Hospital', 'Requires urgent abdominal imaging and surgical review.', seed_now - interval '1 hour');

    INSERT INTO public.Prescriptions (Id, ConsultationNoteId, EncounterId, DrugName, Dosage, Frequency, Duration, Route, Instructions, Status, IssuedAt)
    VALUES
        (pharmacy_prescription_id, pharmacy_consultation_id, pharmacy_encounter_id, 'Artemether/Lumefantrine', '80/480 mg', 'Twice daily', '3 days', 'Oral', 'Take after meals.', 'Pending', seed_now - interval '3 hours'),
        (handover_prescription_id, handover_consultation_id, handover_encounter_id, 'Amoxicillin/Clavulanate', '625 mg', 'Twice daily', '5 days', 'Oral', 'Complete full course.', 'Dispensed', seed_now - interval '2 hours'),
        (discharged_prescription_id, discharged_consultation_id, discharged_encounter_id, 'Metformin', '500 mg', 'Twice daily', '30 days', 'Oral', 'Take with food.', 'HandedOver', seed_now - interval '1 day');

    INSERT INTO public.Dispensings (Id, PrescriptionId, PharmacistId, DrugName, QuantityDispensed, BatchNumber, ExpiryDate, Notes, DispensedAt)
    VALUES
        (handover_dispensing_id, handover_prescription_id, pharmacist_id, 'Amoxicillin/Clavulanate', 10, 'AMX-TEST-001', tomorrow, 'Packed and awaiting protocol handover.', seed_now - interval '1 hour'),
        (discharged_dispensing_id, discharged_prescription_id, pharmacist_id, 'Metformin', 60, 'MET-TEST-001', (today + interval '18 months')::date, 'One-month refill dispensed.', seed_now - interval '2 hours');

    INSERT INTO public.DrugHandovers (Id, DispensingId, EncounterId, ProtocolOfficerId, PatientNameVerified, DrugListVerified, DosageCounsellingDone, DurationCounsellingDone, CounsellingNotes, HandoverAt)
    VALUES
        (handover_handover_id, handover_dispensing_id, handover_encounter_id, NULL, false, false, false, false, NULL, NULL),
        (discharged_handover_id, discharged_dispensing_id, discharged_encounter_id, protocol_officer_id, true, true, true, true, 'Patient understood dose timing and follow-up date.', seed_now - interval '1 hour');

    INSERT INTO public.LabRequests (Id, ConsultationNoteId, EncounterId, TestName, ClinicalIndication, Status, RequestedAt)
    VALUES
        (lab_request_id, lab_consultation_id, lab_encounter_id, 'Full Blood Count', 'Assess suspected anemia.', 'Pending', seed_now - interval '3 hours'),
        (discharged_lab_request_id, discharged_consultation_id, discharged_encounter_id, 'Fasting Blood Glucose', 'Diabetes follow-up.', 'Completed', seed_now - interval '1 day');

    INSERT INTO public.LabResults (Id, LabRequestId, ScientistId, TestName, Findings, Conclusion, "values", CompletedAt)
    VALUES
        (discharged_lab_result_id, discharged_lab_request_id, scientist_id, 'Fasting Blood Glucose', 'Fasting glucose mildly elevated.', 'Review medication adherence and continue current plan.', '[{"Parameter":"Glucose","Value":"138","Unit":"mg/dL","ReferenceRange":"70-99"}]', seed_now - interval '3 hours');

    INSERT INTO public.DressingOrders (Id, ConsultationNoteId, EncounterId, Instructions, Status, PerformedBy, ProcedureNotes, CompletedAt, CreatedAt)
    VALUES
        (dressing_order_id, dressing_consultation_id, dressing_encounter_id, 'Clean wound with normal saline, apply povidone iodine, and cover with sterile gauze.', 'Pending', NULL, NULL, NULL, seed_now - interval '2 hours'),
        (discharged_dressing_order_id, discharged_consultation_id, discharged_encounter_id, 'Inspect foot, clean minor abrasion, and apply protective dressing.', 'Completed', dressing_nurse_id, 'No discharge or signs of infection. Dressing completed.', seed_now - interval '2 hours', seed_now - interval '1 day');

    INSERT INTO public.ContactTraces (Id, EncounterId, RecordedBy, NextOfKinName, NextOfKinPhone, NextOfKinRelationship, ResidentialAddress, WorkplaceAddress, DischargeNotes, ReferralDestination, RecordedAt)
    VALUES
        (discharged_contact_trace_id, discharged_encounter_id, protocol_officer_id, 'Tolu Adeyemi', '08039000006', 'Son', '9 Market Lane, Ibadan', 'Adeyemi Stores, Dugbe Market', 'Discharged with diabetes care advice and outpatient follow-up.', NULL, seed_now - interval '1 hour'),
        (referred_contact_trace_id, referred_encounter_id, protocol_officer_id, 'Aisha Abdullahi', '08039000007', 'Sister', '31 Crescent Road, Kano', 'Kano Textile Market', 'Referred after emergency stabilization.', 'St. Raphael Specialist Hospital', seed_now - interval '20 minutes');
END
$seed$;

COMMIT;

SELECT
    (SELECT count(*) FROM public.Patients WHERE Id::text LIKE '20000000-0000-0000-0000-00000000000%') AS SeedPatients,
    (SELECT count(*) FROM public.Encounters WHERE Id::text LIKE '30000000-0000-0000-0000-00000000000%') AS SeedEncounters,
    (SELECT count(*) FROM public.Prescriptions WHERE Id IN ('60000000-0000-0000-0000-000000000002', '60000000-0000-0000-0000-000000000005', '60000000-0000-0000-0000-000000000006')) AS SeedPrescriptions,
    (SELECT count(*) FROM public.LabRequests WHERE Id IN ('90000000-0000-0000-0000-000000000003', '90000000-0000-0000-0000-000000000006')) AS SeedLabRequests,
    (SELECT count(*) FROM public.DressingOrders WHERE Id IN ('a0000000-0000-0000-0000-000000000004', 'a0000000-0000-0000-0000-000000000006')) AS SeedDressingOrders,
    (SELECT count(*) FROM public.DrugHandovers WHERE Id IN ('80000000-0000-0000-0000-000000000005', '80000000-0000-0000-0000-000000000006')) AS SeedHandovers;
