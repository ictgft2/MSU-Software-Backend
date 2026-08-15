CREATE OR REPLACE FUNCTION public.usp_LabResult_GetByEncounter(uuid)
RETURNS SETOF public.LabResults
LANGUAGE sql
STABLE
AS $function$
    SELECT result.*
    FROM public.LabResults result
    JOIN public.LabRequests request ON request.Id = result.LabRequestId
    WHERE request.EncounterId = $1
    ORDER BY result.CompletedAt DESC;
$function$;
