CREATE OR REPLACE FUNCTION public.usp_VitalSigns_Insert(
    uuid, uuid, uuid, integer, integer, integer, numeric(5,2), integer, integer, numeric(8,2), varchar(1000), timestamptz)
RETURNS SETOF public.VitalSigns
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.VitalSigns (
        Id, EncounterId, RecordedBy, BloodPressureSystolic, BloodPressureDiastolic, PulseRate,
        Temperature, Spo2, RespiratoryRate, Weight, Notes, RecordedAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12)
    RETURNING *;
$function$;
