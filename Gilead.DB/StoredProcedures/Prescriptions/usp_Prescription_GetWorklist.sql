CREATE OR REPLACE FUNCTION public.usp_Prescription_GetWorklist(varchar(30), date)
RETURNS SETOF public.Prescriptions
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.Prescriptions
    WHERE ($1 IS NULL OR Status = $1)
      AND ($2 IS NULL OR (IssuedAt AT TIME ZONE 'UTC')::date = $2)
    ORDER BY IssuedAt;
$function$;
