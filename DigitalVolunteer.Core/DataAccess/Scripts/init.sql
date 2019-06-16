CREATE SCHEMA IF NOT EXISTS dv;

CREATE TABLE dv.users
(
    id uuid NOT NULL PRIMARY KEY,
	first_name character varying NOT NULL,
	last_name character varying NOT NULL,
	description character varying,
    email character varying NOT NULL UNIQUE,
    password character varying NOT NULL,
    status integer NOT NULL DEFAULT 0,
    is_admin boolean NOT NULL DEFAULT FALSE,
    registration_date timestamp without time zone,
	is_executor boolean NOT NULL DEFAULT FALSE
);

CREATE TABLE dv.categories
(
    id uuid NOT NULL PRIMARY KEY,
    name character varying UNIQUE NOT NULL
);

CREATE TABLE dv.tasks
(
    id uuid NOT NULL PRIMARY KEY,
    title character varying NOT NULL,
    description character varying,
    start_date timestamp without time zone,
    end_date timestamp without time zone,
    contact_phone character varying NOT NULL,
    task_format integer NOT NULL,
    has_push_notifications boolean NOT NULL,
    is_only_for_executors boolean NOT NULL,
	owner_id uuid NOT NULL,
	executor_id uuid,
	CONSTRAINT tasks__executor_id__users_id FOREIGN KEY (executor_id)
        REFERENCES dv.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT tasks__owner_id__users_id FOREIGN KEY (owner_id)
        REFERENCES dv.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);