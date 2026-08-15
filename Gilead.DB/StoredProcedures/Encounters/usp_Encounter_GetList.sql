CREATE OR REPLACE FUNCTION public.usp_Encounter_GetList(varchar(40), date, varchar(30))
RETURNS SETOF public.Encounters
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.Encounters
    WHERE ($1 IS NULL OR Status = $1)
      AND ($2 IS NULL OR (CreatedAt AT TIME ZONE 'UTC')::date = $2)
      AND ($3 IS NULL OR AdmissionType = $3)
    ORDER BY CreatedAt DESC;
$function$;
