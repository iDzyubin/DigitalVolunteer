CREATE SCHEMA IF NOT EXISTS dv;

CREATE TABLE dv.users
(
    id uuid NOT NULL PRIMARY KEY,
	first_name character varying NOT NULL,
	last_name character varying NOT NULL,
	description character varying,
    email character varying NOT NULL UNIQUE,
    password character varying NOT NULL,
	phone character varying,
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

CREATE TABLE dv.digital_tasks
(
    id uuid NOT NULL PRIMARY KEY,
    title character varying NOT NULL,
	category_id uuid NOT NULL,
	owner_id uuid NOT NULL,
	executor_id uuid,
    description character varying,
    start_date timestamp without time zone,
    end_date timestamp without time zone,
    status integer NOT NULL,
    contact_phone character varying NOT NULL,
    task_format integer NOT NULL,
    has_push_notifications boolean NOT NULL,
    is_only_for_executors boolean NOT NULL,
    FOREIGN KEY (category_id)
        REFERENCES dv.categories (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    FOREIGN KEY (owner_id)
        REFERENCES dv.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    FOREIGN KEY (executor_id)
        REFERENCES dv.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE TABLE dv.task_executors
(
    task_id uuid NOT NULL,
    user_id uuid NOT NULL,
    PRIMARY KEY (task_id, user_id),
    FOREIGN KEY (task_id)
        REFERENCES dv.digital_tasks (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    FOREIGN KEY (user_id)
        REFERENCES dv.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

INSERT INTO dv.users(id, first_name, last_name, description, email, password, phone, status, is_admin, registration_date, is_executor) VALUES
	('0867fed9-71b2-418b-8cc6-24d4f4e5b9a3', 'Администратор', 'Основной', NULL, 'admin@admin.com', 'ihNuAXnzW0RjFfO2flJgOrCi1rbybN2tqce2nWJvoyGWclW6', NULL, 1, true, '2000-01-01 00:00:00.000000', false);

INSERT INTO dv.categories(id, name) VALUES
	('c8a70bae-2c53-48b1-8ab0-422c547b59aa', 'Разработка веб-сайта'),
	('de3c61c2-dadc-4175-a5b4-02a6bd3c3ac3', 'Тестирование веб-сайта');