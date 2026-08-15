CREATE OR REPLACE FUNCTION public.usp_Patient_GetById(uuid)
RETURNS SETOF public.Patients
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Patients WHERE Id = $1;
$function$;
