CREATE OR REPLACE FUNCTION public.usp_Prescription_AllHandedOverForEncounter(uuid)
RETURNS boolean
LANGUAGE sql
STABLE
AS $function$
    SELECT EXISTS (
        SELECT 1 FROM public.Prescriptions WHERE EncounterId = $1
    ) AND NOT EXISTS (
        SELECT 1 FROM public.Prescriptions WHERE EncounterId = $1 AND Status <> 'HandedOver'
    );
$function$;
