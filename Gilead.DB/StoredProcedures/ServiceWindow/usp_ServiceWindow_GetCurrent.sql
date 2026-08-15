CREATE OR REPLACE FUNCTION public.usp_ServiceWindow_GetCurrent(date)
RETURNS SETOF public.ServiceTimeWindows
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.ServiceTimeWindows WHERE Date = $1;
$function$;
