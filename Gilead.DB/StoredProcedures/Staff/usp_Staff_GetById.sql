CREATE OR REPLACE FUNCTION public.usp_Staff_GetById(uuid)
RETURNS SETOF public.Staff
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Staff WHERE Id = $1;
$function$;
