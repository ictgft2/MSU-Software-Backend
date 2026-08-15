CREATE OR REPLACE FUNCTION public.usp_LabRequest_GetWorklist(varchar(30), date)
RETURNS SETOF public.LabRequests
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.LabRequests
    WHERE ($1 IS NULL OR Status = $1)
      AND ($2 IS NULL OR (RequestedAt AT TIME ZONE 'UTC')::date = $2)
    ORDER BY RequestedAt;
$function$;
