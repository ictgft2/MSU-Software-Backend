CREATE OR REPLACE FUNCTION public.usp_Prescription_GetById(uuid)
RETURNS SETOF public.Prescriptions
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Prescriptions WHERE Id = $1;
$function$;
