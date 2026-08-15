CREATE OR REPLACE FUNCTION public.usp_ServiceWindow_Update(uuid, time, time)
RETURNS SETOF public.ServiceTimeWindows
LANGUAGE sql
VOLATILE
AS $function$
    UPDATE public.ServiceTimeWindows
    SET ColdCaseOpenTime = $2, ColdCaseCloseTime = $3
    WHERE Id = $1
    RETURNING *;
$function$;
