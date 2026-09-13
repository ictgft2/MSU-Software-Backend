CREATE OR REPLACE FUNCTION public.usp_Staff_Insert(uuid, varchar(200), varchar(320), varchar(30), boolean, timestamptz)
RETURNS SETOF public.Staff
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.Staff (Id, FullName, Email, Role, IsActive, CreatedAt)
    VALUES ($1, $2, $3, $4, $5, $6)
    RETURNING *;
$function$;
