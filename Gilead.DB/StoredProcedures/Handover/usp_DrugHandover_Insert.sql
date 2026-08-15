CREATE OR REPLACE FUNCTION public.usp_DrugHandover_Insert(
    uuid, uuid, uuid, uuid, boolean, boolean, boolean, boolean, varchar(1000), timestamptz)
RETURNS SETOF public.DrugHandovers
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.DrugHandovers (
        Id, DispensingId, EncounterId, ProtocolOfficerId, PatientNameVerified, DrugListVerified,
        DosageCounsellingDone, DurationCounsellingDone, CounsellingNotes, HandoverAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
    RETURNING *;
$function$;
