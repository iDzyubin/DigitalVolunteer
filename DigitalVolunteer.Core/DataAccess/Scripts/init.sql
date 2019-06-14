CREATE SCHEMA IF NOT EXISTS dv;

CREATE TABLE dv.users
(
    id uuid NOT NULL PRIMARY KEY,
    email character varying NOT NULL UNIQUE,
    password character varying NOT NULL,
    status integer NOT NULL DEFAULT 0,
    is_admin boolean NOT NULL DEFAULT FALSE,
    registration_date timestamp without time zone
);

CREATE TABLE dv.categories
(
    id uuid NOT NULL PRIMARY KEY,
    name character varying UNIQUE NOT NULL
)