\set ON_ERROR_STOP on

DO $verify_reads$
BEGIN
    IF (SELECT count(*) FROM public.usp_Patient_GetById('20000000-0000-0000-0000-000000000001')) <> 1 THEN
        RAISE EXCEPTION 'Patient lookup verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Patient_Search('amina', NULL)) <> 1 THEN
        RAISE EXCEPTION 'Case-insensitive patient search verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Encounter_GetById('30000000-0000-0000-0000-000000000001')) <> 1
       OR (SELECT count(*) FROM public.usp_Encounter_GetList(NULL, NULL, NULL)) < 7 THEN
        RAISE EXCEPTION 'Encounter read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_VitalSigns_GetByEncounter('30000000-0000-0000-0000-000000000001')) <> 1
       OR (SELECT count(*) FROM public.usp_VitalSigns_GetLatest('30000000-0000-0000-0000-000000000001')) <> 1 THEN
        RAISE EXCEPTION 'Vital-sign read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Consultation_GetByEncounter('30000000-0000-0000-0000-000000000002')) <> 1 THEN
        RAISE EXCEPTION 'Consultation read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Prescription_GetById('60000000-0000-0000-0000-000000000002')) <> 1
       OR (SELECT count(*) FROM public.usp_Prescription_GetWorklist(NULL, NULL)) < 3
       OR NOT public.usp_Prescription_AllHandedOverForEncounter('30000000-0000-0000-0000-000000000006') THEN
        RAISE EXCEPTION 'Prescription read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Dispensing_GetById('70000000-0000-0000-0000-000000000006')) <> 1 THEN
        RAISE EXCEPTION 'Dispensing read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_DrugHandover_GetById('80000000-0000-0000-0000-000000000006')) <> 1
       OR (SELECT count(*) FROM public.usp_DrugHandover_GetWorklist('Pending')) <> 1 THEN
        RAISE EXCEPTION 'Drug-handover read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_LabRequest_GetById('90000000-0000-0000-0000-000000000003')) <> 1
       OR (SELECT count(*) FROM public.usp_LabRequest_GetWorklist(NULL, NULL)) < 2
       OR (SELECT count(*) FROM public.usp_LabResult_GetByEncounter('30000000-0000-0000-0000-000000000006')) <> 1 THEN
        RAISE EXCEPTION 'Lab read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_DressingOrder_GetById('a0000000-0000-0000-0000-000000000004')) <> 1
       OR (SELECT count(*) FROM public.usp_DressingOrder_GetWorklist('Pending')) <> 1 THEN
        RAISE EXCEPTION 'Dressing read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_ContactTrace_GetByEncounter('30000000-0000-0000-0000-000000000006')) <> 1 THEN
        RAISE EXCEPTION 'Contact-trace read verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_Register_GetDrugs(NULL, 1, 50)) <> 1
       OR (SELECT count(*) FROM public.usp_Register_ExportDrugs(NULL)) <> 1 THEN
        RAISE EXCEPTION 'Drug-register pagination/export verification failed';
    END IF;
    IF (SELECT count(*) FROM public.usp_ServiceWindow_GetCurrent((CURRENT_TIMESTAMP AT TIME ZONE 'UTC')::date)) <> 1 THEN
        RAISE EXCEPTION 'Service-window read verification failed';
    END IF;
END
$verify_reads$;

BEGIN;

DO $verify_writes$
DECLARE
    patient_id uuid := 'd0000000-0000-0000-0000-000000000001';
    encounter_id uuid := 'd0000000-0000-0000-0000-000000000002';
    vital_id uuid := 'd0000000-0000-0000-0000-000000000003';
    consultation_id uuid := 'd0000000-0000-0000-0000-000000000004';
    prescription_id uuid := 'd0000000-0000-0000-0000-000000000005';
    lab_request_id uuid := 'd0000000-0000-0000-0000-000000000006';
    dressing_id uuid := 'd0000000-0000-0000-0000-000000000007';
    dispensing_id uuid := 'd0000000-0000-0000-0000-000000000008';
    handover_id uuid := 'd0000000-0000-0000-0000-000000000009';
    result_id uuid := 'd0000000-0000-0000-0000-000000000010';
    trace_id uuid := 'd0000000-0000-0000-0000-000000000011';
    window_id uuid := 'd0000000-0000-0000-0000-000000000012';
    staff_id uuid := 'd0000000-0000-0000-0000-000000000099';
    source_time timestamptz := '2026-08-15 12:00:00+01';
BEGIN
    PERFORM public.usp_Patient_Insert(patient_id, 'Migration Test', 40, 'F', '08000000000', 'Test Address', 'Test Kin', '08000000001', 'Sibling', source_time);
    PERFORM public.usp_Encounter_Insert(encounter_id, patient_id, 'ColdCase', 'Queued', 'WalkedIn', 'Migration verification', staff_id, source_time, NULL, source_time, source_time);
    PERFORM public.usp_VitalSigns_Insert(vital_id, encounter_id, staff_id, 120, 80, 70, 37.25, 99, 16, 12345.67, NULL, source_time);
    PERFORM public.usp_Consultation_Insert(consultation_id, encounter_id, staff_id, '["Verified"]', 'Verified', true, true, false, NULL, NULL, source_time);

    PERFORM public.usp_Prescription_InsertBulk(jsonb_build_array(jsonb_build_object(
        'Id', prescription_id, 'ConsultationNoteId', consultation_id, 'EncounterId', encounter_id,
        'DrugName', 'Verification Drug', 'Dosage', '1 tablet', 'Frequency', 'Daily', 'Duration', '1 day',
        'Route', 'Oral', 'Instructions', NULL, 'Status', 'Pending', 'IssuedAt', source_time)));
    PERFORM public.usp_LabRequest_InsertBulk(jsonb_build_array(jsonb_build_object(
        'Id', lab_request_id, 'ConsultationNoteId', consultation_id, 'EncounterId', encounter_id,
        'TestName', 'Verification Test', 'ClinicalIndication', 'Migration', 'Status', 'Pending', 'RequestedAt', source_time)));
    PERFORM public.usp_DressingOrder_Insert(dressing_id, consultation_id, encounter_id, 'Verification dressing', 'Pending', NULL, NULL, NULL, source_time);
    PERFORM public.usp_Dispensing_Insert(dispensing_id, prescription_id, staff_id, 'Verification Drug', 1, 'VERIFY-1', '2099-01-01', NULL, source_time);
    PERFORM public.usp_DrugHandover_Insert(handover_id, dispensing_id, encounter_id, NULL, false, false, false, false, NULL, NULL);
    PERFORM public.usp_DrugHandover_Confirm(handover_id, dispensing_id, encounter_id, staff_id, true, true, true, true, 'Verified', source_time);
    PERFORM public.usp_LabResult_Insert(result_id, lab_request_id, staff_id, 'Verification Test', 'Normal', 'Verified', '[]', source_time);
    PERFORM public.usp_ContactTrace_Insert(trace_id, encounter_id, staff_id, 'Test Kin', '08000000001', 'Sibling', 'Home', 'Work', 'Verified', NULL, source_time);
    PERFORM public.usp_ContactTrace_Update(trace_id, encounter_id, staff_id, staff_id, 'Updated Kin', '08000000001', 'Sibling', 'Home', 'Work', 'Verified', NULL, source_time);
    PERFORM public.usp_ServiceWindow_Insert(window_id, '2099-01-01', '08:00', '17:00', staff_id, source_time);
    PERFORM public.usp_ServiceWindow_Update(window_id, '09:00', '18:00');
    PERFORM public.usp_Prescription_UpdateStatus(prescription_id, 'HandedOver');
    PERFORM public.usp_DressingOrder_Complete(dressing_id, staff_id, 'Completed');
    PERFORM public.usp_Encounter_UpdateStatus(encounter_id, 'Discharged', source_time);

    IF (SELECT CreatedAt FROM public.Patients WHERE Id = patient_id) <> '2026-08-15 11:00:00+00'::timestamptz THEN
        RAISE EXCEPTION 'timestamptz UTC normalization verification failed';
    END IF;
    IF (SELECT Weight FROM public.VitalSigns WHERE Id = vital_id) <> 12345.67::numeric(8,2) THEN
        RAISE EXCEPTION 'numeric precision verification failed';
    END IF;
    IF (SELECT Status FROM public.LabRequests WHERE Id = lab_request_id) <> 'Completed'
       OR (SELECT Status FROM public.Prescriptions WHERE Id = prescription_id) <> 'HandedOver'
       OR (SELECT Status FROM public.DressingOrders WHERE Id = dressing_id) <> 'Completed'
       OR (SELECT Status FROM public.Encounters WHERE Id = encounter_id) <> 'Discharged' THEN
        RAISE EXCEPTION 'Transactional workflow update verification failed';
    END IF;
END
$verify_writes$;

ROLLBACK;

DO $verify_rollback$
BEGIN
    IF EXISTS (SELECT 1 FROM public.Patients WHERE Id = 'd0000000-0000-0000-0000-000000000001') THEN
        RAISE EXCEPTION 'Transaction rollback verification failed';
    END IF;
END
$verify_rollback$;

SELECT 'PostgreSQL migration verification passed' AS result;
