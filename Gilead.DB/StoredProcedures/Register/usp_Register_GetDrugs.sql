CREATE OR REPLACE FUNCTION public.usp_Register_GetDrugs(date, integer, integer)
RETURNS SETOF public.vw_DrugRegister
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.vw_DrugRegister
    WHERE $1 IS NULL OR (HandoverAt AT TIME ZONE 'UTC')::date = $1
    ORDER BY HandoverAt DESC
    LIMIT $3 OFFSET (($2 - 1) * $3);
$function$;
