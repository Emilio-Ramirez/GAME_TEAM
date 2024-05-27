const { sequelize, Carta, Receta } = require('./db');

const cartas = [
  { id_carta: 1, nombre: 'Whole Wheat Bread', valor_nutrimental: 4, tipo: 'Side' },
  { id_carta: 2, nombre: 'Chicken', valor_nutrimental: 2, tipo: 'Protein' },
  { id_carta: 3, nombre: 'Tongs', valor_nutrimental: 3, tipo: 'Utils' },
  { id_carta: 4, nombre: 'Grill', valor_nutrimental: 2, tipo: 'Utils' },
  { id_carta: 5, nombre: 'Feta Cheese', valor_nutrimental: 3, tipo: 'Vegetable' },
  { id_carta: 6, nombre: 'Cherry Tomatoes', valor_nutrimental: 2, tipo: 'Vegetable' },
  { id_carta: 7, nombre: 'Egg', valor_nutrimental: 8, tipo: 'Protein' },
  { id_carta: 8, nombre: 'Ham', valor_nutrimental: 7, tipo: 'Protein' },
  { id_carta: 9, nombre: 'Onion', valor_nutrimental: 6, tipo: 'Vegetable' },
  { id_carta: 10, nombre: 'Bacon', valor_nutrimental: 7, tipo: 'Vegetable' },
  { id_carta: 11, nombre: 'Knife', valor_nutrimental: 6, tipo: 'Utils' },
  { id_carta: 12, nombre: 'Bowl', valor_nutrimental: 8, tipo: 'Utils' },
  { id_carta: 13, nombre: 'Olive Oil', valor_nutrimental: 7, tipo: 'Side' },
  { id_carta: 14, nombre: 'Salt', valor_nutrimental: 5, tipo: 'Side' },
  { id_carta: 15, nombre: 'Turkey', valor_nutrimental: 3, tipo: 'Protein' },
  { id_carta: 16, nombre: 'Mashed Potatoes', valor_nutrimental: 4, tipo: 'Side' },
  { id_carta: 17, nombre: 'Carrot', valor_nutrimental: 4, tipo: 'Vegetable' },
  { id_carta: 18, nombre: 'Brussels Sprouts', valor_nutrimental: 3, tipo: 'Vegetable' },
  { id_carta: 19, nombre: 'Oven', valor_nutrimental: 3, tipo: 'Utils' },
  { id_carta: 20, nombre: 'Pot', valor_nutrimental: 4, tipo: 'Utils' },
  { id_carta: 21, nombre: 'Wooden Spoon', valor_nutrimental: 4, tipo: 'Utils' },
  { id_carta: 22, nombre: 'Rice', valor_nutrimental: 3, tipo: 'Side' },
  { id_carta: 23, nombre: 'Cranberry Sauce', valor_nutrimental: 5, tipo: 'Side' },
  { id_carta: 24, nombre: 'Pork', valor_nutrimental: 8, tipo: 'Protein' },
  { id_carta: 25, nombre: 'Cutting Board', valor_nutrimental: 6, tipo: 'Utils' },
  { id_carta: 26, nombre: 'Thermometer', valor_nutrimental: 8, tipo: 'Utils' },
  { id_carta: 27, nombre: 'Mustard', valor_nutrimental: 6, tipo: 'Side' },
  { id_carta: 28, nombre: 'Apple Cider', valor_nutrimental: 7, tipo: 'Side' },
  { id_carta: 29, nombre: 'Salmon', valor_nutrimental: 3, tipo: 'Protein' },
  { id_carta: 30, nombre: 'Heavy Cream', valor_nutrimental: 4, tipo: 'Side' },
  { id_carta: 31, nombre: 'Pan', valor_nutrimental: 2, tipo: 'Utils' },
  { id_carta: 32, nombre: 'Mixer', valor_nutrimental: 3, tipo: 'Utils' },
  { id_carta: 33, nombre: 'Lemon', valor_nutrimental: 2, tipo: 'Side' },
  { id_carta: 34, nombre: 'Asparagus', valor_nutrimental: 3, tipo: 'Vegetable' },
  { id_carta: 35, nombre: 'Chocolate', valor_nutrimental: 4, tipo: 'Protein' },
  { id_carta: 36, nombre: 'Strawberry', valor_nutrimental: 3, tipo: 'Vegetable' },
  { id_carta: 37, nombre: 'Rosemary', valor_nutrimental: 3, tipo: 'Vegetable' },
  { id_carta: 38, nombre: 'Butter', valor_nutrimental: 7, tipo: 'Side' },
  { id_carta: 39, nombre: 'Sugar', valor_nutrimental: 6, tipo: 'Side' },
  { id_carta: 40, nombre: 'Flour', valor_nutrimental: 8, tipo: 'Protein' },
  { id_carta: 41, nombre: 'Rolling Pin', valor_nutrimental: 6, tipo: 'Utils' },
  { id_carta: 42, nombre: 'Molds', valor_nutrimental: 7, tipo: 'Utils' },
  { id_carta: 43, nombre: 'White Fondant', valor_nutrimental: 9, tipo: 'Vegetable' },
  { id_carta: 44, nombre: 'Raspberry Jam', valor_nutrimental: 7, tipo: 'Vegetable' },
  { id_carta: 45, nombre: 'Olives', valor_nutrimental: 4, tipo: 'Side' },
  { id_carta: 46, nombre: 'Salad', valor_nutrimental: 3, tipo: 'Vegetable' }
];


const recetas = [
  // Picnic
  {
    is_main: true,
    ingredientes: {
      side: ['Olive oil', 'Salt'],
      verduras: ['Onion', 'Bacon'],
      protein: ['Egg', 'Ham'],
      utils: ['Knife', 'Bowl']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Whole wheat bread'],
      verduras: ['Feta cheese', 'Cherry tomatoes'],
      protein: ['Chicken'],
      utils: ['Tongs', 'Grill']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Black olives', 'Avocado'],
      verduras: ['Cherry tomatoes', 'Salad'],
      protein: ['Chicken'],
      utils: ['Pan', 'Tongs']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Black olives'],
      verduras: ['Salad', 'Feta cheese'],
      protein: ['Cheese'],
      utils: ['Grill', 'Pan']
    }
  },
  // Christmas Dinner
  {
    is_main: true,
    ingredientes: {
      side: ['Mustard', 'Apple cider'],
      verduras: ['Rosemary', 'Onion'],
      protein: ['Pork'],
      utils: ['Cutting board', 'Meat thermometer']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Mashed potatoes', 'Cranberry sauce'],
      verduras: ['Glazed carrots', 'Brussels sprouts'],
      protein: ['Turkey'],
      utils: ['Wooden spoon', 'Tongs']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Rice', 'Cranberry sauce'],
      verduras: ['Salad', 'Asparagus'],
      protein: ['Ham'],
      utils: ['Tongs', 'Pot']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Rice', 'Cranberry rice'],
      verduras: ['Salad', 'Asparagus'],
      protein: ['Turkey'],
      utils: ['Tongs', 'Pot']
    }
  },
  // Wedding
  {
    is_main: true,
    ingredientes: {
      side: ['Butter', 'White sugar'],
      verduras: ['White fondant', 'Raspberry jam'],
      protein: ['Flour', 'Eggs'],
      utils: ['Rolling pin', 'Molds']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Lemon', 'Heavy cream'],
      verduras: ['Rosemary', 'Salad'],
      protein: ['Salmon'],
      utils: ['Pan', 'Whisk']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Potatoes', 'Lemon'],
      verduras: ['Strawberries', 'Salad'],
      protein: ['Salmon'],
      utils: ['Oven', 'Pan']
    }
  },
  {
    is_main: false,
    ingredientes: {
      side: ['Heavy cream'],
      verduras: ['Strawberries', 'Cranberry sauce'],
      protein: ['Dark chocolate'],
      utils: ['Mixer', 'Wooden spoon']
    }
  }
];

async function populateDatabase() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Eliminar todas las cartas y recetas existentes utilizando CASCADE
    await sequelize.query('TRUNCATE "Carta" CASCADE');
    await sequelize.query('TRUNCATE "Receta" CASCADE');

    // Reiniciar el contador de la secuencia de la tabla "Receta"
    await sequelize.query('ALTER SEQUENCE "Receta_id_receta_seq" RESTART WITH 1');

    // Insertar las nuevas cartas en la base de datos
    await Carta.bulkCreate(cartas);
    console.log('Cartas successfully populated');

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
