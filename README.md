# ProductionApp
MVVM app with ORM and MySQL

Commands of creating DataBase:

CREATE DATABASE db;

CREATE TABLE `Employee` ( `id` INT NOT NULL AUTO_INCREMENT , `lastName` VARCHAR(70) NOT NULL , `name` VARCHAR(70) NOT NULL ,
`secondName` VARCHAR(70) NOT NULL , `birthDate` DATE NOT NULL , `gender` ENUM('Male','Female') NOT NULL , `subdivision` INT , PRIMARY KEY (`id`));

CREATE TABLE `Subdivision` ( `id` INT NOT NULL AUTO_INCREMENT , `name` VARCHAR(100) NOT NULL , `employee` INT, PRIMARY KEY (`id`));
