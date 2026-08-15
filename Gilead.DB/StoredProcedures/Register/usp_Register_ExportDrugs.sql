CREATE OR REPLACE FUNCTION public.usp_Register_ExportDrugs(date)
RETURNS SETOF public.vw_DrugRegister
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.vw_DrugRegister
    WHERE $1 IS NULL OR (HandoverAt AT TIME ZONE 'UTC')::date = $1
    ORDER BY HandoverAt DESC;
$function$;
