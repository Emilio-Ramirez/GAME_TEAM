USE Recipe_Rumble;

INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) 
VALUES (TRUE, 1, '{"side": ["Olive Oil", "Salt"], "verduras": ["Onion", "Bacon"], "protein": ["Egg", "Ham"], "utils": ["Knife", "Bowl"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) 
VALUES (FALSE, 1, '{"side": ["Bread"], "verduras": ["Feta Cheese", "Cherry Tomatoes"], "protein": ["Chicken"], "utils": ["Tongs", "Grill"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) 
VALUES (FALSE, 1, '{"side": ["Olives", "Bread"], "verduras": ["Cherry Tomatoes", "Salad"], "protein": ["Chicken"], "utils": ["Pan", "Tongs"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) 
VALUES (FALSE, 1, '{"side": ["Olives"], "verduras": ["Salad", "Feta Cheese"], "protein": ["Jam"], "utils": ["Grill", "Pan"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes) 
VALUES (TRUE, 2, '{"side": ["Mustard", "Apple Cider"], "verduras": ["Rosemary", "Onion"], "protein": ["Pork"], "utils": ["Cutting Board", "Thermometer"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 2, '{"side": ["Potatoes", "Cranberry Sauce"], "verduras": ["Carrot", "Brussels Sprouts"], "protein": ["Turkey"], "utils": ["Wooden Spoon", "Tongs"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 2, '{"side": ["Rice", "Potatoes"], "verduras": ["Salad", "Brussels Sprouts"], "protein": ["Ham"], "utils": ["Wooden Spoon", "Pot"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 2, '{"side": ["Rice", "Cranberry Sauce"], "verduras": ["Salad", "Asparagus"], "protein": ["Turkey"], "utils": ["Tongs", "Pot"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (TRUE, 3, '{"side": ["Butter", "Sugar"], "verduras": ["White Fondant", "Jam"], "protein": ["Flour", "Egg"], "utils": ["Rolling Pin", "Molds"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 3, '{"side": ["Lemon", "Heavy Cream"], "verduras": ["Rosemary", "Salad"], "protein": ["Salmon"], "utils": ["Pan", "Mixer"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 3, '{"side": ["Potatoes", "Lemon"], "verduras": ["Strawberry", "Salad"], "protein": ["Salmon"], "utils": ["Oven", "Pan"]}');
INSERT INTO Receta (es_principal, belongs_to_level, ingredientes)
VALUES (FALSE, 3, '{"side": ["Heavy Cream"], "verduras": ["Strawberry", "Cranberry Sauce"], "protein": ["Chocolate"], "utils": ["Mixer", "Wooden Spoon"]}');

