CREATE TABLE IF NOT EXISTS public.Staff (
    Id uuid NOT NULL,
    FullName varchar(200) NOT NULL,
    Email varchar(320) NOT NULL,
    Role varchar(30) NOT NULL,
    IsActive boolean NOT NULL,
    PasswordHash varchar(500) NULL,
    CreatedAt timestamptz NOT NULL,
    CONSTRAINT pk_staff PRIMARY KEY (Id),
    CONSTRAINT uq_staff_email UNIQUE (Email),
    CONSTRAINT ck_staff_role CHECK (Role IN ('Doctor', 'Pharmacist', 'Nurse', 'Scientist', 'ProtocolOfficer', 'Registrar', 'DressingNurse'))
);

CREATE INDEX IF NOT EXISTS ix_staff_role_active ON public.Staff (Role, IsActive);

INSERT INTO public.Staff (Id, FullName, Email, Role, IsActive, CreatedAt)
VALUES ('00000000-0000-0000-0000-000000000001', 'System Migration', 'system-migration@gilead.local', 'Registrar', true, CURRENT_TIMESTAMP)
ON CONFLICT (Id) DO NOTHING;

ALTER TABLE public.Encounters ADD COLUMN IF NOT EXISTS RegisteredBy uuid;
ALTER TABLE public.VitalSigns ADD COLUMN IF NOT EXISTS RecordedBy uuid;
ALTER TABLE public.ContactTraces ADD COLUMN IF NOT EXISTS RecordedBy uuid;
ALTER TABLE public.ServiceTimeWindows ADD COLUMN IF NOT EXISTS CreatedBy uuid;
ALTER TABLE public.Staff ADD COLUMN IF NOT EXISTS PasswordHash varchar(500);

UPDATE public.Encounters SET RegisteredBy = '00000000-0000-0000-0000-000000000001' WHERE RegisteredBy IS NULL;
UPDATE public.VitalSigns SET RecordedBy = '00000000-0000-0000-0000-000000000001' WHERE RecordedBy IS NULL;
UPDATE public.ContactTraces SET RecordedBy = '00000000-0000-0000-0000-000000000001' WHERE RecordedBy IS NULL;
UPDATE public.ServiceTimeWindows SET CreatedBy = '00000000-0000-0000-0000-000000000001' WHERE CreatedBy IS NULL;

ALTER TABLE public.Encounters ALTER COLUMN RegisteredBy SET NOT NULL;
ALTER TABLE public.VitalSigns ALTER COLUMN RecordedBy SET NOT NULL;
ALTER TABLE public.ContactTraces ALTER COLUMN RecordedBy SET NOT NULL;
ALTER TABLE public.ServiceTimeWindows ALTER COLUMN CreatedBy SET NOT NULL;

DO $migration$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_encounters_registered_by') THEN
        ALTER TABLE public.Encounters
            ADD CONSTRAINT fk_encounters_registered_by FOREIGN KEY (RegisteredBy) REFERENCES public.Staff (Id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_vitalsigns_recorded_by') THEN
        ALTER TABLE public.VitalSigns
            ADD CONSTRAINT fk_vitalsigns_recorded_by FOREIGN KEY (RecordedBy) REFERENCES public.Staff (Id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_contacttraces_recorded_by') THEN
        ALTER TABLE public.ContactTraces
            ADD CONSTRAINT fk_contacttraces_recorded_by FOREIGN KEY (RecordedBy) REFERENCES public.Staff (Id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_servicetimewindows_created_by') THEN
        ALTER TABLE public.ServiceTimeWindows
            ADD CONSTRAINT fk_servicetimewindows_created_by FOREIGN KEY (CreatedBy) REFERENCES public.Staff (Id);
    END IF;
END
$migration$;
