CREATE OR REPLACE FUNCTION public.usp_DressingOrder_GetById(uuid)
RETURNS SETOF public.DressingOrders
LANGUAGE sql
STABLE
AS $function$
    SELECT * FROM public.DressingOrders WHERE Id = $1;
$function$;
