CREATE OR REPLACE FUNCTION public.usp_Staff_Update(uuid, varchar(200), varchar(320), varchar(30), boolean)
RETURNS SETOF public.Staff
LANGUAGE sql
VOLATILE
AS $function$
    UPDATE public.Staff
    SET FullName = $2,
        Email = $3,
        Role = $4,
        IsActive = $5
    WHERE Id = $1
    RETURNING *;
$function$;
