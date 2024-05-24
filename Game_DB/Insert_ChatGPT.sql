USE Recipe_Rumble;

-- Insertar registros en la tabla Usuario
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event) VALUES (1000, 'Novato', 2.5);
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event) VALUES (1500, 'Intermedio', 3.2);
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event) VALUES (2000, 'Avanzado', 4.1);
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event) VALUES (2500, 'Experto', 5.0);
INSERT INTO Usuario (puntaje_maximo, usr_rank, average_dishes_per_event) VALUES (3000, 'Maestro', 6.3);

-- Insertar registros en la tabla Receta (con un valor TRUE por cada nivel)
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (TRUE, 1, '{"ingredientes": ["pollo", "arroz", "pimientos"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (FALSE, 1, '{"ingredientes": ["tomate", "queso", "albahaca"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (TRUE, 2, '{"ingredientes": ["carne", "patatas", "cebolla"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (FALSE, 2, '{"ingredientes": ["zanahoria", "pepino", "lechuga"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (TRUE, 3, '{"ingredientes": ["pescado", "limón", "perejil"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (FALSE, 3, '{"ingredientes": ["calabacín", "champiñones", "ajo"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (TRUE, 4, '{"ingredientes": ["cerdo", "manzana", "canela"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (FALSE, 4, '{"ingredientes": ["espinacas", "huevo", "queso"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (TRUE, 5, '{"ingredientes": ["cordero", "menta", "yogur"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) VALUES (FALSE, 5, '{"ingredientes": ["berenjena", "pimientos", "salsa de soja"]}');

-- Insertar registros en la tabla Nivel
INSERT INTO Nivel (titulo, id_receta_principal) VALUES ('Nivel 1: Principiante', 1);
INSERT INTO Nivel (titulo, id_receta_principal) VALUES ('Nivel 2: Intermedio', 3);
INSERT INTO Nivel (titulo, id_receta_principal) VALUES ('Nivel 3: Avanzado', 5);
INSERT INTO Nivel (titulo, id_receta_principal) VALUES ('Nivel 4: Experto', 7);
INSERT INTO Nivel (titulo, id_receta_principal) VALUES ('Nivel 5: Maestro', 9);

-- Insertar registros en la tabla Partida
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel) VALUES ('2024-05-01', 1200, 1, 1);
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel) VALUES ('2024-05-02', 1400, 2, 2);
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel) VALUES ('2024-05-03', 1600, 3, 3);
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel) VALUES ('2024-05-04', 1800, 4, 4);
INSERT INTO Partida (fecha, puntaje, id_usuario, id_nivel) VALUES ('2024-05-05', 2000, 5, 5);

-- Insertar registros en la tabla Ingrediente
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('pollo', 5, 'carne', 1);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('arroz', 3, 'grano', 1);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('pimientos', 2, 'vegetal', 1);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('tomate', 4, 'vegetal', 2);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('queso', 6, 'lácteo', 2);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('albahaca', 1, 'hierba', 2);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('carne', 5, 'carne', 3);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('patatas', 3, 'vegetal', 3);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('cebolla', 2, 'vegetal', 3);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('zanahoria', 4, 'vegetal', 4);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('pepino', 3, 'vegetal', 4);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('lechuga', 1, 'vegetal', 4);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('pescado', 6, 'carne', 5);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('limón', 2, 'fruta', 5);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('perejil', 1, 'hierba', 5);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('calabacín', 3, 'vegetal', 6);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('champiñones', 4, 'vegetal', 6);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('ajo', 2, 'hierba', 6);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('cerdo', 5, 'carne', 7);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('manzana', 3, 'fruta', 7);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('canela', 1, 'especia', 7);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('espinacas', 2, 'vegetal', 8);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('huevo', 4, 'proteína', 8);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('queso', 6, 'lácteo', 8);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('cordero', 5, 'carne', 9);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('menta', 2, 'hierba', 9);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('yogur', 3, 'lácteo', 9);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('berenjena', 4, 'vegetal', 10);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('pimientos', 2, 'vegetal', 10);
INSERT INTO Ingrediente (nombre, puntaje, tipo, id_receta) VALUES ('salsa de soja', 1, 'condimento', 10);

-- Insertar registros en la tabla Sesion
INSERT INTO Sesion (token, fecha_inicio, fecha_expiracion, ultima_actividad, id_usuario) VALUES ('abc123', '2024-05-01 10:00:00', '2024-05-01 12:00:00', '2024-05-01 11:00:00', 1);
INSERT INTO Sesion (token, fecha_inicio, fecha_expiracion, ultima_actividad, id_usuario) VALUES ('def456', '2024-05-02 10:00:00', '2024-05-02 12:00:00', '2024-05-02 12:00:00', 1);
