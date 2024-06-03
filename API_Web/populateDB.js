const { sequelize, Receta, Ingrediente } = require('./db');

const ingredientes = [
  { nombre: 'Bread', puntaje: 4, tipo: 'Side' },
  { nombre: 'Chicken', puntaje: 2, tipo: 'Protein' },
  { nombre: 'Tongs', puntaje: 3, tipo: 'Utils' },
  { nombre: 'Grill', puntaje: 2, tipo: 'Utils' },
  { nombre: 'Feta Cheese', puntaje: 3, tipo: 'Vegetable' },
  { nombre: 'Cherry Tomatoes', puntaje: 2, tipo: 'Vegetable' },
  { nombre: 'Egg', puntaje: 8, tipo: 'Protein' },
  { nombre: 'Ham', puntaje: 7, tipo: 'Protein' },
  { nombre: 'Onion', puntaje: 6, tipo: 'Vegetable' },
  { nombre: 'Bacon', puntaje: 7, tipo: 'Vegetable' },
  { nombre: 'Knife', puntaje: 6, tipo: 'Utils' },
  { nombre: 'Bowl', puntaje: 8, tipo: 'Utils' },
  { nombre: 'Olive Oil', puntaje: 7, tipo: 'Side' },
  { nombre: 'Salt', puntaje: 5, tipo: 'Side' },
  { nombre: 'Turkey', puntaje: 3, tipo: 'Protein' },
  { nombre: 'Potatoes', puntaje: 4, tipo: 'Side' },
  { nombre: 'Carrot', puntaje: 4, tipo: 'Vegetable' },
  { nombre: 'Brussels Sprouts', puntaje: 3, tipo: 'Vegetable' },
  { nombre: 'Oven', puntaje: 3, tipo: 'Utils' },
  { nombre: 'Pot', puntaje: 4, tipo: 'Utils' },
  { nombre: 'Wooden Spoon', puntaje: 4, tipo: 'Utils' },
  { nombre: 'Rice', puntaje: 3, tipo: 'Side' },
  { nombre: 'Cranberry Sauce', puntaje: 5, tipo: 'Side' },
  { nombre: 'Pork', puntaje: 8, tipo: 'Protein' },
  { nombre: 'Cutting Board', puntaje: 6, tipo: 'Utils' },
  { nombre: 'Thermometer', puntaje: 8, tipo: 'Utils' },
  { nombre: 'Mustard', puntaje: 6, tipo: 'Side' },
  { nombre: 'Apple Cider', puntaje: 7, tipo: 'Side' },
  { nombre: 'Salmon', puntaje: 3, tipo: 'Protein' },
  { nombre: 'Heavy Cream', puntaje: 4, tipo: 'Side' },
  { nombre: 'Pan', puntaje: 2, tipo: 'Utils' },
  { nombre: 'Mixer', puntaje: 3, tipo: 'Utils' },
  { nombre: 'Lemon', puntaje: 2, tipo: 'Side' },
  { nombre: 'Asparagus', puntaje: 3, tipo: 'Vegetable' },
  { nombre: 'Chocolate', puntaje: 4, tipo: 'Protein' },
  { nombre: 'Strawberry', puntaje: 3, tipo: 'Vegetable' },
  { nombre: 'Rosemary', puntaje: 3, tipo: 'Vegetable' },
  { nombre: 'Butter', puntaje: 7, tipo: 'Side' },
  { nombre: 'Sugar', puntaje: 6, tipo: 'Side' },
  { nombre: 'Flour', puntaje: 8, tipo: 'Protein' },
  { nombre: 'Rolling Pin', puntaje: 6, tipo: 'Utils' },
  { nombre: 'Molds', puntaje: 7, tipo: 'Utils' },
  { nombre: 'White Fondant', puntaje: 9, tipo: 'Vegetable' },
  { nombre: 'Jam', puntaje: 7, tipo: 'Vegetable' },
  { nombre: 'Olives', puntaje: 4, tipo: 'Side' },
  { nombre: 'Salad', puntaje: 3, tipo: 'Vegetable' }
];

const recetas = [
  // Picnic
  {
    es_principal: true,
    belongs_to_level: 1,
    ingredientes: {
      side: ['Olive Oil', 'Salt'],
      verduras: ['Onion', 'Bacon'],
      protein: ['Egg', 'Ham'],
      utils: ['Knife', 'Bowl']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 1,
    ingredientes: {
      side: ['Bread'],
      verduras: ['Feta Cheese', 'Cherry Tomatoes'],
      protein: ['Chicken'],
      utils: ['Tongs', 'Grill']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 1,
    ingredientes: {
      side: ['Olives'],
      verduras: ['Cherry Tomatoes', 'Salad'],
      protein: ['Chicken'],
      utils: ['Pan', 'Tongs']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 1,
    ingredientes: {
      side: ['Olives'],
      verduras: ['Salad', 'Feta Cheese'],
      protein: [],
      utils: ['Grill', 'Pan']
    }
  },
  // Christmas Dinner
  {
    es_principal: true,
    belongs_to_level: 2,
    ingredientes: {
      side: ['Mustard', 'Apple Cider'],
      verduras: ['Rosemary', 'Onion'],
      protein: ['Pork'],
      utils: ['Cutting Board', 'Thermometer']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 2,
    ingredientes: {
      side: ['Potatoes', 'Cranberry Sauce'],
      verduras: ['Carrot', 'Brussels Sprouts'],
      protein: ['Turkey'],
      utils: ['Wooden Spoon', 'Tongs']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 2,
    ingredientes: {
      side: ['Rice', 'Cranberry Sauce'],
      verduras: ['Salad', 'Asparagus'],
      protein: ['Ham'],
      utils: ['Tongs', 'Pot']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 2,
    ingredientes: {
      side: ['Rice', 'Cranberry Sauce'],
      verduras: ['Salad', 'Asparagus'],
      protein: ['Turkey'],
      utils: ['Tongs', 'Pot']
    }
  },
  // Wedding
  {
    es_principal: true,
    belongs_to_level: 3,
    ingredientes: {
      side: ['Butter', 'Sugar'],
      verduras: ['White Fondant', 'Jam'],
      protein: ['Flour', 'Egg'],
      utils: ['Rolling Pin', 'Molds']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 3,
    ingredientes: {
      side: ['Lemon', 'Heavy Cream'],
      verduras: ['Rosemary', 'Salad'],
      protein: ['Salmon'],
      utils: ['Pan']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 3,
    ingredientes: {
      side: ['Potatoes', 'Lemon'],
      verduras: ['Strawberry', 'Salad'],
      protein: ['Salmon'],
      utils: ['Oven', 'Pan']
    }
  },
  {
    es_principal: false,
    belongs_to_level: 3,
    ingredientes: {
      side: ['Heavy Cream'],
      verduras: ['Strawberry', 'Cranberry Sauce'],
      protein: ['Chocolate'],
      utils: ['Mixer', 'Wooden Spoon']
    }
  }
];

async function populateDatabase() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Eliminar todas las cartas y recetas existentes utilizando CASCADE
    await sequelize.query('TRUNCATE TABLE "Ingredientes" CASCADE');
    await sequelize.query('TRUNCATE TABLE "Receta" CASCADE');

    // Reiniciar el contador de la secuencia de la tabla "Receta"
    await sequelize.query('ALTER SEQUENCE "Receta_id_receta_seq" RESTART WITH 1');

    // Insertar los nuevos ingredientes en la base de datos
    await Ingrediente.bulkCreate(ingredientes);
    console.log('Ingredientes successfully populated');

    // Insertar las nuevas recetas en la base de datos
    await Receta.bulkCreate(recetas);
    console.log('Recetas successfully populated');

    await sequelize.close();
    console.log('Database connection closed');
  } catch (error) {
    console.error('Error populating database:', error);
  }
}

populateDatabase();
