CREATE OR REPLACE FUNCTION public.usp_Consultation_Insert(
    uuid, uuid, uuid, text, text, boolean, boolean, boolean, varchar(250), varchar(1000), timestamptz)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH inserted AS (
        INSERT INTO public.ConsultationNotes (
            Id, EncounterId, DoctorId, Diagnosis, ClinicalNotes, RequiresLab, RequiresDressing,
            IsReferral, ReferralFacility, ReferralReason, ConsultedAt)
        VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11)
        RETURNING 1
    )
    SELECT count(*)::integer FROM inserted;
$function$;
