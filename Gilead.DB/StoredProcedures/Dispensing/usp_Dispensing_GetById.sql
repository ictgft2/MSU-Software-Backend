CREATE OR REPLACE FUNCTION public.usp_Dispensing_GetById(uuid)
RETURNS SETOF public.Dispensings
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Dispensings WHERE Id = $1;
$function$;
