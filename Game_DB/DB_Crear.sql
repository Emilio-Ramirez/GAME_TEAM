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

---
--- Views ---
---

-- Vista 1: Usuarios con su puntaje m‡ximo y nivel
CREATE VIEW v_usuarios_puntaje_nivel AS
SELECT u.username, u.puntaje_maximo, u.usr_rank
FROM USUARIO u;

-- Vista 2: Partidas con detalles de cartas jugadas
CREATE VIEW v_partidas_cartas AS
SELECT p.id_partida, p.fecha, c.nombre, c.tipo, c.valor_nutrimental
FROM PARTIDA p
JOIN CARTAS_EN_PARTIDA cp ON p.id_partida = cp.id_partida
JOIN CARTAS c ON cp.id_carta = c.id_carta;

-- Vista 3: Recetas principales de cada nivel
CREATE VIEW v_recetas_principales AS
SELECT n.titulo, r.id_receta, r.ingredientes
FROM RECETA r
JOIN NIVEL n ON r.belongs_to_level = n.id_nivel
WHERE r.es_principal = true;

-- Vista 4: Usuarios con nœmero de partidas jugadas
CREATE VIEW v_usuarios_partidas AS
SELECT u.username, COUNT(p.id_partida) AS partidas_jugadas
FROM USUARIO u
LEFT JOIN PARTIDA p ON u.id_usuario = p.id_usuario
GROUP BY u.username;

-- Vista 5: Cartas m‡s utilizadas en partidas
CREATE VIEW v_cartas_mas_utilizadas AS
SELECT c.nombre, c.tipo, COUNT(cp.id_carta_en_partida) AS veces_utilizadas
FROM CARTAS c
JOIN CARTAS_EN_PARTIDA cp ON c.id_carta = cp.id_carta
GROUP BY c.id_carta
ORDER BY veces_utilizadas DESC
LIMIT 10;

-- Vista 6: Recetas por nivel
CREATE VIEW v_recetas_por_nivel AS
SELECT n.titulo, r.id_receta, r.es_principal, r.ingredientes
FROM RECETA r
JOIN NIVEL n ON r.belongs_to_level = n.id_nivel;

-- Vista 7: Sesiones activas
CREATE VIEW v_sesiones_activas AS
SELECT s.token, u.username, s.fecha_inicio, s.ultima_actividad
FROM SESION s
JOIN USUARIO u ON s.id_usuario = u.id_usuario
WHERE s.fecha_expiracion >= CURDATE();

----
--- Procedimientos almacenados ---
----

-- Procedimiento 1: Obtener cartas de una partida
CREATE PROCEDURE sp_obtener_cartas_partida(
    IN p_id_partida INT
)
BEGIN
    SELECT c.nombre, c.valor_nutrimental, c.tipo
    FROM CARTAS c
    JOIN CARTAS_EN_PARTIDA cp ON c.id_carta = cp.id_carta
    WHERE cp.id_partida = p_id_partida;
END;

-- Procedimiento 2: Actualizar puntaje m‡ximo de un usuario
CREATE PROCEDURE sp_actualizar_puntaje_maximo(
    IN p_id_usuario INT,
    IN p_nuevo_puntaje INT
)
BEGIN
    UPDATE USUARIO
    SET puntaje_maximo = p_nuevo_puntaje
    WHERE id_usuario = p_id_usuario;
END;

-- Procedimiento 3: Obtener recetas de un nivel
CREATE PROCEDURE sp_obtener_recetas_nivel(
    IN p_id_nivel INT
)
BEGIN
    SELECT r.id_receta, r.es_principal, r.ingredientes
    FROM RECETA r
    WHERE r.belongs_to_level = p_id_nivel;
END;

---
---  Triggers
---

-- Disparador 1: Actualizar promedio de platos por evento
CREATE TRIGGER tr_actualizar_promedio_platos
AFTER INSERT ON PARTIDA
FOR EACH ROW
BEGIN
    UPDATE USUARIO
    SET average_dishes_per_event = (
        SELECT AVG(COUNT(r.id_receta))
        FROM PARTIDA p
        JOIN CARTAS_EN_PARTIDA cp ON p.id_partida = cp.id_partida
        JOIN CARTAS c ON cp.id_carta = c.id_carta
        JOIN RECETA r ON c.id_receta = r.id_receta
        WHERE p.id_usuario = NEW.id_usuario
        GROUP BY p.id_partida
    )
    WHERE id_usuario = NEW.id_usuario;
END;

-- Disparador 2: Evitar insertar cartas duplicadas en partida
CREATE TRIGGER tr_evitar_cartas_duplicadas
BEFORE INSERT ON CARTAS_EN_PARTIDA
FOR EACH ROW
BEGIN
    IF EXISTS (
        SELECT 1
        FROM CARTAS_EN_PARTIDA
        WHERE id_partida = NEW.id_partida
        AND id_carta = NEW.id_carta
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se pueden insertar cartas duplicadas en una partida';
    END IF;
END;

-- Disparador 3: Verificar puntaje m‡ximo en nueva partida
CREATE TRIGGER tr_verificar_puntaje_partida
BEFORE INSERT ON PARTIDA
FOR EACH ROW
BEGIN
    DECLARE puntaje_maximo INT;
    SELECT puntaje_maximo INTO puntaje_maximo
    FROM USUARIO
    WHERE id_usuario = NEW.id_usuario;

    IF NEW.puntaje > puntaje_maximo THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El puntaje de la partida excede el puntaje maximo del usuario';
    END IF;
END;

---
--- Events 
---

-- Evento 1: Eliminar sesiones expiradas
CREATE EVENT ev_eliminar_sesiones_expiradas
ON SCHEDULE EVERY 1 DAY
DO
BEGIN
    DELETE FROM SESION
    WHERE fecha_expiracion < CURDATE();
END;

-- Evento 2: Actualizar œltima actividad de sesiones
CREATE EVENT ev_actualizar_ultima_actividad
ON SCHEDULE EVERY 1 HOUR
DO
BEGIN
    UPDATE SESION
    SET ultima_actividad = CURRENT_TIMESTAMP
    WHERE ultima_actividad <= DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 HOUR);
END;
