CREATE OR REPLACE FUNCTION public.usp_LabRequest_GetById(uuid)
RETURNS SETOF public.LabRequests
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.LabRequests WHERE Id = $1;
$function$;
