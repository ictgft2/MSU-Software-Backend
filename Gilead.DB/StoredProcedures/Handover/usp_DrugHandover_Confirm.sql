CREATE OR REPLACE FUNCTION public.usp_DrugHandover_Confirm(
    uuid, uuid, uuid, uuid, boolean, boolean, boolean, boolean, varchar(1000), timestamptz)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH updated AS (
        UPDATE public.DrugHandovers
        SET ProtocolOfficerId = $4,
            PatientNameVerified = $5,
            DrugListVerified = $6,
            DosageCounsellingDone = $7,
            DurationCounsellingDone = $8,
            CounsellingNotes = $9,
            HandoverAt = $10
        WHERE Id = $1 AND DispensingId = $2 AND EncounterId = $3
        RETURNING 1
    )
    SELECT count(*)::integer FROM updated;
$function$;
