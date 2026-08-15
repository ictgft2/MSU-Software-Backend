CREATE OR REPLACE FUNCTION public.usp_ServiceWindow_Insert(uuid, date, time, time, uuid, timestamptz)
RETURNS SETOF public.ServiceTimeWindows
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.ServiceTimeWindows (Id, Date, ColdCaseOpenTime, ColdCaseCloseTime, CreatedBy, CreatedAt)
    VALUES ($1, $2, $3, $4, $5, $6)
    RETURNING *;
$function$;
