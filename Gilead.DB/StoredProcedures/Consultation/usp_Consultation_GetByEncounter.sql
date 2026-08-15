CREATE OR REPLACE FUNCTION public.usp_Consultation_GetByEncounter(uuid)
RETURNS SETOF public.ConsultationNotes
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.ConsultationNotes
    WHERE EncounterId = $1;
$function$;
