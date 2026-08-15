CREATE OR REPLACE FUNCTION public.usp_DressingOrder_Complete(uuid, uuid, text)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH updated AS (
        UPDATE public.DressingOrders
        SET Status = 'Completed',
            PerformedBy = $2,
            ProcedureNotes = $3,
            CompletedAt = CURRENT_TIMESTAMP
        WHERE Id = $1
        RETURNING 1
    )
    SELECT count(*)::integer FROM updated;
$function$;
