CREATE OR REPLACE FUNCTION public.usp_VitalSigns_GetByEncounter(uuid)
RETURNS SETOF public.VitalSigns
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.VitalSigns WHERE EncounterId = $1 ORDER BY RecordedAt DESC;
$function$;
