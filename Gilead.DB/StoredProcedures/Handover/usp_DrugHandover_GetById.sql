CREATE OR REPLACE FUNCTION public.usp_DrugHandover_GetById(uuid)
RETURNS SETOF public.DrugHandovers
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.DrugHandovers WHERE Id = $1;
$function$;
