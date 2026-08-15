CREATE OR REPLACE FUNCTION public.usp_Encounter_GetById(uuid)
RETURNS SETOF public.Encounters
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Encounters WHERE Id = $1;
$function$;
