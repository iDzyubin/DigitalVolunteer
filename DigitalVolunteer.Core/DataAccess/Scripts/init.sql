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
);

CREATE TABLE dv.accounts
(
    id uuid NOT NULL PRIMARY KEY,
    first_name character varying NOT NULL,
    last_name character varying NOT NULL,
    description character varying NOT NULL
);

CREATE TABLE dv.tasks
(
    id uuid NOT NULL PRIMARY KEY,
    title character varying NOT NULL,
    description character varying,
    start_date date NOT NULL,
    end_date date,
    contact_phone character varying NOT NULL,
    task_format integer NOT NULL,
    has_push_notifications boolean NOT NULL,
    is_only_for_executors boolean NOT NULL,
	owner_id uuid NOT NULL,
	executor_id uuid NOT NULL
);

CREATE TABLE dv.task_owners_executors
(
    task_id uuid NOT NULL,
    owner_id uuid NOT NULL,
    executor_id uuid NOT NULL,
    CONSTRAINT task_owners_executors__executor_id__accounts__id FOREIGN KEY (executor_id)
        REFERENCES dv.accounts (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT task_owners_executors__owner_id__accounts__id FOREIGN KEY (owner_id)
        REFERENCES dv.accounts (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT task_owners_executors__task_id__tasks__id FOREIGN KEY (task_id)
        REFERENCES dv.tasks (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);