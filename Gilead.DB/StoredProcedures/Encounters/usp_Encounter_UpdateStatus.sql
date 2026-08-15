CREATE OR REPLACE FUNCTION public.usp_Encounter_UpdateStatus(uuid, varchar(40), timestamptz)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH updated AS (
        UPDATE public.Encounters
        SET Status = $2,
            DischargedAt = COALESCE($3, DischargedAt),
            UpdatedAt = CURRENT_TIMESTAMP
        WHERE Id = $1
        RETURNING 1
    )
    SELECT count(*)::integer FROM updated;
$function$;
