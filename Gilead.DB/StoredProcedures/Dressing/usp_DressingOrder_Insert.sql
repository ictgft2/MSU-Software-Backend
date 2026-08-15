CREATE OR REPLACE FUNCTION public.usp_DressingOrder_Insert(
    uuid, uuid, uuid, text, varchar(30), uuid, text, timestamptz, timestamptz)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH inserted AS (
        INSERT INTO public.DressingOrders (
            Id, ConsultationNoteId, EncounterId, Instructions, Status, PerformedBy, ProcedureNotes, CompletedAt, CreatedAt)
        VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
        RETURNING 1
    )
    SELECT count(*)::integer FROM inserted;
$function$;
