CREATE OR REPLACE FUNCTION public.usp_Dispensing_Insert(
    uuid, uuid, uuid, varchar(200), integer, varchar(100), date, varchar(1000), timestamptz)
RETURNS SETOF public.Dispensings
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.Dispensings (
        Id, PrescriptionId, PharmacistId, DrugName, QuantityDispensed, BatchNumber, ExpiryDate, Notes, DispensedAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
    RETURNING *;
$function$;
