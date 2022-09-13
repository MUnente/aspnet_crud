CREATE DATABASE people_crud;
USE people_crud;

CREATE TABLE role (
	id INT IDENTITY(1,1) PRIMARY KEY,
	description VARCHAR(20) NOT NULL
);

CREATE TABLE person (
	id INT IDENTITY(1,1) PRIMARY KEY,
	username VARCHAR(20) NOT NULL,
	fullname VARCHAR(60) NOT NULL,
	fulldate VARCHAR(9) NOT NULL,
	active BIT NOT NULL,
	country VARCHAR(30) NOT NULL,
	roleId INT NOT NULL FOREIGN KEY REFERENCES role(id)
);

INSERT INTO role (description) VALUES ('Admin'), ('Customer'), ('Developer');