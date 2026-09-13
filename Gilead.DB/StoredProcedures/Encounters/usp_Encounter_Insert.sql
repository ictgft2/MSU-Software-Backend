CREATE OR REPLACE FUNCTION public.usp_Encounter_Insert(
    uuid, uuid, varchar(30), varchar(40), varchar(30), varchar(1000), uuid,
    timestamptz, timestamptz, timestamptz, timestamptz)
RETURNS SETOF public.Encounters
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.Encounters (
        Id, PatientId, AdmissionType, Status, ArrivalMode, ChiefComplaint, RegisteredBy,
        AdmittedAt, DischargedAt, CreatedAt, UpdatedAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11)
    RETURNING *;
$function$;
