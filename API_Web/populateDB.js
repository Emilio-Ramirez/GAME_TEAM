const { sequelize, Receta, Cartas } = require('./db');

const cards = [
  { nombre: 'Bread', valor_nutrimental: 4, tipo: 'Side' },
  { nombre: 'Chicken', valor_nutrimental: 2, tipo: 'Protein' },
  { nombre: 'Tongs', valor_nutrimental: 3, tipo: 'Utils' },
  { nombre: 'Grill', valor_nutrimental: 2, tipo: 'Utils' },
  { nombre: 'Feta Cheese', valor_nutrimental: 3, tipo: 'Vegetable' },
  { nombre: 'Cherry Tomatoes', valor_nutrimental: 2, tipo: 'Vegetable' },
  { nombre: 'Egg', valor_nutrimental: 8, tipo: 'Protein' },
  { nombre: 'Ham', valor_nutrimental: 7, tipo: 'Protein' },
  { nombre: 'Onion', valor_nutrimental: 6, tipo: 'Vegetable' },
  { nombre: 'Bacon', valor_nutrimental: 7, tipo: 'Vegetable' },
  { nombre: 'Knife', valor_nutrimental: 6, tipo: 'Utils' },
  { nombre: 'Bowl', valor_nutrimental: 8, tipo: 'Utils' },
  { nombre: 'Olive Oil', valor_nutrimental: 7, tipo: 'Side' },
  { nombre: 'Salt', valor_nutrimental: 5, tipo: 'Side' },
  { nombre: 'Turkey', valor_nutrimental: 3, tipo: 'Protein' },
  { nombre: 'Potatoes', valor_nutrimental: 4, tipo: 'Side' },
  { nombre: 'Carrot', valor_nutrimental: 4, tipo: 'Vegetable' },
  { nombre: 'Brussels Sprouts', valor_nutrimental: 3, tipo: 'Vegetable' },
  { nombre: 'Oven', valor_nutrimental: 3, tipo: 'Utils' },
  { nombre: 'Pot', valor_nutrimental: 4, tipo: 'Utils' },
  { nombre: 'Wooden Spoon', valor_nutrimental: 4, tipo: 'Utils' },
  { nombre: 'Rice', valor_nutrimental: 3, tipo: 'Side' },
  { nombre: 'Cranberry Sauce', valor_nutrimental: 5, tipo: 'Side' },
  { nombre: 'Pork', valor_nutrimental: 8, tipo: 'Protein' },
  { nombre: 'Cutting Board', valor_nutrimental: 6, tipo: 'Utils' },
  { nombre: 'Thermometer', valor_nutrimental: 8, tipo: 'Utils' },
  { nombre: 'Mustard', valor_nutrimental: 6, tipo: 'Side' },
  { nombre: 'Apple Cider', valor_nutrimental: 7, tipo: 'Side' },
  { nombre: 'Salmon', valor_nutrimental: 3, tipo: 'Protein' },
  { nombre: 'Heavy Cream', valor_nutrimental: 4, tipo: 'Side' },
  { nombre: 'Pan', valor_nutrimental: 2, tipo: 'Utils' },
  { nombre: 'Mixer', valor_nutrimental: 3, tipo: 'Utils' },
  { nombre: 'Lemon', valor_nutrimental: 2, tipo: 'Side' },
  { nombre: 'Asparagus', valor_nutrimental: 3, tipo: 'Vegetable' },
  { nombre: 'Chocolate', valor_nutrimental: 4, tipo: 'Protein' },
  { nombre: 'Strawberry', valor_nutrimental: 3, tipo: 'Vegetable' },
  { nombre: 'Rosemary', valor_nutrimental: 3, tipo: 'Vegetable' },
  { nombre: 'Butter', valor_nutrimental: 7, tipo: 'Side' },
  { nombre: 'Sugar', valor_nutrimental: 6, tipo: 'Side' },
  { nombre: 'Flour', valor_nutrimental: 8, tipo: 'Protein' },
  { nombre: 'Rolling Pin', valor_nutrimental: 6, tipo: 'Utils' },
  { nombre: 'Molds', valor_nutrimental: 7, tipo: 'Utils' },
  { nombre: 'White Fondant', valor_nutrimental: 9, tipo: 'Vegetable' },
  { nombre: 'Jam', valor_nutrimental: 7, tipo: 'Vegetable' },
  { nombre: 'Olives', valor_nutrimental: 4, tipo: 'Side' },
  { nombre: 'Salad', valor_nutrimental: 3, tipo: 'Vegetable' }
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
    await sequelize.query('TRUNCATE TABLE "Cartas" CASCADE');
    await sequelize.query('TRUNCATE TABLE "Receta" CASCADE');

    // Reiniciar el contador de la secuencia de la tabla "Receta"
    await sequelize.query('ALTER SEQUENCE "Receta_id_receta_seq" RESTART WITH 1');

    // Insertar los nuevos ingredientes en la base de datos
    await Cartas.bulkCreate(cards);
    console.log('Cards successfully populated');

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
