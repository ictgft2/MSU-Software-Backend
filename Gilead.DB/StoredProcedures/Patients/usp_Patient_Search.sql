CREATE OR REPLACE FUNCTION public.usp_Patient_Search(varchar(200), varchar(40))
RETURNS SETOF public.Patients
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.Patients
    WHERE ($1 IS NULL OR FullName ILIKE '%' || $1 || '%')
      AND ($2 IS NULL OR Phone = $2)
    ORDER BY CreatedAt DESC;
$function$;
