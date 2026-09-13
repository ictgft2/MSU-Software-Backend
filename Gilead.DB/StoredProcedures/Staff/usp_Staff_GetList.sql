CREATE OR REPLACE FUNCTION public.usp_Staff_GetList(varchar(30), boolean)
RETURNS SETOF public.Staff
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.Staff
    WHERE ($1 IS NULL OR Role = $1)
      AND ($2 IS NULL OR IsActive = $2)
    ORDER BY FullName;
$function$;
