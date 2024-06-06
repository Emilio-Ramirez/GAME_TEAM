-- Crear base de datos
CREATE DATABASE IF NOT EXISTS Recipe_Rumble;
USE Recipe_Rumble;

-- Tabla Usuario
CREATE TABLE IF NOT EXISTS Usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    puntaje_maximo INT,
    usr_rank VARCHAR(20),
    average_dishes_per_event FLOAT,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL
);

-- Tabla Receta
CREATE TABLE IF NOT EXISTS Receta (
    id_receta INT AUTO_INCREMENT PRIMARY KEY,
    es_principal BOOLEAN DEFAULT FALSE,
    belongs_to_level INT,
    ingredientes JSON
);

-- Tabla Nivel
CREATE TABLE IF NOT EXISTS Nivel (
    id_nivel INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(20)
);

-- Tabla Partida
CREATE TABLE IF NOT EXISTS Partida (
    id_partida INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    puntaje INT
);

-- Tabla Cartas
CREATE TABLE IF NOT EXISTS Cartas (
    id_carta INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(20),
    valor_nutrimental INT,
    tipo VARCHAR(20)
);

-- Tabla Sesion
CREATE TABLE IF NOT EXISTS Sesion (
    id_sesion INT AUTO_INCREMENT PRIMARY KEY,
    token VARCHAR(20) UNIQUE,
    fecha_inicio DATE,
    fecha_expiracion DATE,
    ultima_actividad DATE
);

