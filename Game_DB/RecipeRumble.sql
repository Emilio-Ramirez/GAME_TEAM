
CREATE DATABASE IF NOT EXISTS Recipe_Rumble;
USE Recipe_Rumble;

CREATE TABLE Usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    puntaje_maximo INT,
    usr_rank VARCHAR(50),
    average_dishes_per_event FLOAT,
    username VARCHAR(50),
    password_ VARCHAR(200)
);


CREATE TABLE Receta (
    id_receta INT AUTO_INCREMENT PRIMARY KEY,
    es_principal BOOLEAN,
    belongs_to_level INT,
    ingredientes JSON
);

CREATE TABLE Nivel (
    id_nivel INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(100),
    id_receta_principal INT,
    FOREIGN KEY (id_receta_principal) REFERENCES Receta(id_receta)
);

CREATE TABLE Partida (
    id_partida INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    puntaje INT,
    id_usuario INT,
    id_nivel INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_nivel) REFERENCES Nivel(id_nivel)
);

CREATE TABLE Ingrediente (
    id_carta INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100),
    puntaje INT,
    tipo VARCHAR(50),
    id_receta INT,
    FOREIGN KEY (id_receta) REFERENCES Receta(id_receta)
);

CREATE TABLE Sesion (
    id_sesion INT AUTO_INCREMENT PRIMARY KEY,
    token VARCHAR(255),
    fecha_inicio DATETIME,
    fecha_expiracion DATETIME,
    ultima_actividad DATETIME,
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);