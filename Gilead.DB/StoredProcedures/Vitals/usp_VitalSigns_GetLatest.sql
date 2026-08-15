CREATE OR REPLACE FUNCTION public.usp_VitalSigns_GetLatest(uuid)
RETURNS SETOF public.VitalSigns
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.VitalSigns WHERE EncounterId = $1 ORDER BY RecordedAt DESC LIMIT 1;
$function$;
