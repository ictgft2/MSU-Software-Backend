CREATE OR REPLACE FUNCTION public.usp_DressingOrder_GetWorklist(varchar(30))
RETURNS SETOF public.DressingOrders
LANGUAGE sql
STABLE
AS $function$
    SELECT *
    FROM public.DressingOrders
    WHERE $1 IS NULL OR Status = $1
    ORDER BY CreatedAt;
$function$;
