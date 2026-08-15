CREATE OR REPLACE FUNCTION public.usp_LabResult_Insert(
    uuid, uuid, uuid, varchar(200), text, text, text, timestamptz)
RETURNS SETOF public.LabResults
LANGUAGE sql
VOLATILE
AS $function$
    WITH inserted AS (
        INSERT INTO public.LabResults (
            Id, LabRequestId, ScientistId, TestName, Findings, Conclusion, "values", CompletedAt)
        VALUES ($1, $2, $3, $4, $5, $6, $7, $8)
        RETURNING *
    ), updated AS (
        UPDATE public.LabRequests request
        SET Status = 'Completed'
        FROM inserted
        WHERE request.Id = inserted.LabRequestId
        RETURNING request.Id
    )
    SELECT inserted.*
    FROM inserted
    CROSS JOIN (SELECT count(*) FROM updated) AS update_result;
$function$;
