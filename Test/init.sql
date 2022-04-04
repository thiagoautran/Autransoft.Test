CREATE SCHEMA IF NOT EXISTS public;

DROP TABLE IF EXISTS public.fii CASCADE;
DROP TABLE IF EXISTS public.action CASCADE;
 
CREATE TABLE IF NOT EXISTS public.action
(
    id uuid NOT NULL,
    company_id int NULL,
    company_name character varying(100) NULL,
    ticker character varying(50) NULL,
    last_update date NULL,
    CONSTRAINT action_pkey PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.fii
(
    id uuid NOT NULL,
    company_id int NULL,
    company_name character varying(100) NULL,
    ticker character varying(50) NULL,
    last_update date NULL,
    CONSTRAINT fii_pkey PRIMARY KEY (id)
);
