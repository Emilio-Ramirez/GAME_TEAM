USE Recipe_Rumble;

-- Insertar datos en la tabla Usuario
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event, username, password_)
VALUES (1200, 'Gold', 5.3, 'user1', 'password1'),
       (1500, 'Platinum', 6.0, 'user2', 'password2'),
       (800, 'Silver', 4.5, 'user3', 'password3'),
       (2000, 'Diamond', 7.8, 'user4', 'password4'),
       (600, 'Bronze', 3.2, 'user5', 'password5');

-- Insertar datos en la tabla Receta
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (TRUE, 1, '{"ingredientes": ["tomate", "queso", "albahaca"]}'),
       (FALSE, 1, '{"ingredientes": ["pollo", "pimienta", "limón"]}'),
       (TRUE, 2, '{"ingredientes": ["carne", "sal", "ajo"]}'),
       (FALSE, 2, '{"ingredientes": ["pescado", "perejil", "aceite"]}'),
       (TRUE, 3, '{"ingredientes": ["huevo", "harina", "leche"]}');

-- Insertar datos en la tabla Nivel
INSERT INTO Nivel (titulo, id_receta_principal)
VALUES ('Nivel 1: Básico', 1),
       ('Nivel 2: Intermedio', 3),
       ('Nivel 3: Avanzado', 5),
       ('Nivel 4: Experto', 1),
       ('Nivel 5: Maestro', 3);

-- Insertar datos en la tabla Partida
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel)
VALUES ('2023-01-01', 100, 1, 1),
       ('2023-02-01', 150, 2, 2),
       ('2023-03-01', 200, 3, 3),
       ('2023-04-01', 250, 4, 4),
       ('2023-05-01', 300, 5, 5);

-- Insertar datos en la tabla Ingrediente
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta)
VALUES ('tomate', 10, 'vegetal', 1),
       ('queso', 15, 'lácteo', 1),
       ('pollo', 20, 'carne', 2),
       ('pimienta', 5, 'especia', 2),
       ('carne', 25, 'carne', 3);

-- Insertar datos en la tabla Sesion
INSERT INTO Sesion (token, fecha_inicio, fecha_expiracion, ultima_actividad, id_usuario)
VALUES ('token1', '2023-01-01 10:00:00', '2023-01-01 12:00:00', '2023-01-01 11:00:00', 1),
       ('token2', '2023-02-01 10:00:00', '2023-02-01 12:00:00', '2023-02-01 11:00:00', 2),
       ('token3', '2023-03-01 10:00:00', '2023-03-01 12:00:00', '2023-03-01 11:00:00', 3),
       ('token4', '2023-04-01 10:00:00', '2023-04-01 12:00:00', '2023-04-01 11:00:00', 4),
       ('token5', '2023-05-01 10:00:00', '2023-05-01 12:00:00', '2023-05-01 11:00:00', 5);
