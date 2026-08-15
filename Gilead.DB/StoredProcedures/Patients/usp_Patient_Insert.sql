CREATE OR REPLACE FUNCTION public.usp_Patient_Insert(
    uuid, varchar(200), integer, varchar(1), varchar(40), varchar(500), varchar(200), varchar(40), varchar(100), timestamptz)
RETURNS SETOF public.Patients
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.Patients (
        Id, FullName, Age, Sex, Phone, Address, NextOfKinName, NextOfKinPhone, NextOfKinRelationship, CreatedAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
    RETURNING *;
$function$;
