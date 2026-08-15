CREATE OR REPLACE FUNCTION public.usp_DrugHandover_GetWorklist(varchar(30))
RETURNS SETOF public.DrugHandovers
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.DrugHandovers
    WHERE $1 IS NULL
       OR ($1 = 'Pending' AND HandoverAt IS NULL)
       OR ($1 = 'Completed' AND HandoverAt IS NOT NULL)
    ORDER BY COALESCE(HandoverAt, 'infinity'::timestamptz), Id;
$function$;
