CREATE SCHEMA IF NOT EXISTS dv;

CREATE TABLE dv.users
(
    id uuid NOT NULL PRIMARY KEY,
    email character varying NOT NULL UNIQUE,
    password character varying NOT NULL,
    status integer NOT NULL DEFAULT 0,
    registration_date timestamp without time zone
);