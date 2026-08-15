CREATE OR REPLACE FUNCTION public.usp_ContactTrace_GetByEncounter(uuid)
RETURNS SETOF public.ContactTraces
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.ContactTraces WHERE EncounterId = $1;
$function$;
