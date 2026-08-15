CREATE OR REPLACE FUNCTION public.usp_Prescription_UpdateStatus(uuid, varchar(30))
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH updated AS (
        UPDATE public.Prescriptions SET Status = $2 WHERE Id = $1 RETURNING 1
    )
    SELECT count(*)::integer FROM updated;
$function$;
