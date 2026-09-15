CREATE OR REPLACE FUNCTION public.usp_Staff_GetByEmail(varchar(320))
RETURNS SETOF public.Staff
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.Staff WHERE lower(Email) = lower($1);
$function$;