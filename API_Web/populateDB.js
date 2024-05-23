const { sequelize, Carta, Receta } = require('./db');

const cartas = [
  { id: 1, nombre: 'Pan integral', valor_nutrimental: 4, tipo: 'side' },
  { id: 2, nombre: 'Pollo', valor_nutrimental: 2, tipo: 'proteina' },
  { id: 3, nombre: 'Pinzas', valor_nutrimental: 3, tipo: 'utils' },
  { id: 4, nombre: 'Parrilla', valor_nutrimental: 2, tipo: 'utils' },
  { id: 5, nombre: 'Queso feta', valor_nutrimental: 3, tipo: 'vegetable' },
  { id: 6, nombre: 'Tomates cherry', valor_nutrimental: 2, tipo: 'vegetable' },
  { id: 7, nombre: 'Huevo', valor_nutrimental: 8, tipo: 'proteina' },
  { id: 8, nombre: 'Jamón', valor_nutrimental: 7, tipo: 'proteina' },
  { id: 9, nombre: 'Cebolla', valor_nutrimental: 6, tipo: 'vegetable' },
  { id: 10, nombre: 'Tocino', valor_nutrimental: 7, tipo: 'vegetable' },
  { id: 11, nombre: 'Cuchillo', valor_nutrimental: 6, tipo: 'utils' },
  { id: 12, nombre: 'Bowl', valor_nutrimental: 8, tipo: 'utils' },
  { id: 13, nombre: 'Aceite de oliva', valor_nutrimental: 7, tipo: 'side' },
  { id: 14, nombre: 'Sal', valor_nutrimental: 5, tipo: 'side' },
  { id: 15, nombre: 'Pavo', valor_nutrimental: 3, tipo: 'proteina' },
  { id: 16, nombre: 'Puré de papas', valor_nutrimental: 4, tipo: 'side' },
  { id: 17, nombre: 'Zanahoria', valor_nutrimental: 4, tipo: 'vegetable' },
  { id: 18, nombre: 'Coles de Bruselas', valor_nutrimental: 3, tipo: 'vegetable' },
  { id: 19, nombre: 'Horno', valor_nutrimental: 3, tipo: 'utils' },
  { id: 20, nombre: 'Olla', valor_nutrimental: 4, tipo: 'utils' },
  { id: 21, nombre: 'Cuchara de madera', valor_nutrimental: 4, tipo: 'utils' },
  { id: 22, nombre: 'Arroz', valor_nutrimental: 3, tipo: 'side' },
  { id: 23, nombre: 'Salsa de arándano', valor_nutrimental: 5, tipo: 'side' },
  { id: 24, nombre: 'Cerdo', valor_nutrimental: 8, tipo: 'proteina' },
  { id: 25, nombre: 'Tabla de cortar', valor_nutrimental: 6, tipo: 'utils' },
  { id: 26, nombre: 'Termómetro', valor_nutrimental: 8, tipo: 'utils' },
  { id: 27, nombre: 'Mostaza', valor_nutrimental: 6, tipo: 'side' },
  { id: 28, nombre: 'Sidra de manzana', valor_nutrimental: 7, tipo: 'side' },
  { id: 29, nombre: 'Salmón', valor_nutrimental: 3, tipo: 'proteina' },
  { id: 30, nombre: 'Crema de leche', valor_nutrimental: 4, tipo: 'side' },
  { id: 31, nombre: 'Sartén', valor_nutrimental: 2, tipo: 'utils' },
  { id: 32, nombre: 'Batidora', valor_nutrimental: 3, tipo: 'utils' },
  { id: 33, nombre: 'Limón', valor_nutrimental: 2, tipo: 'side' },
  { id: 34, nombre: 'Espárragos', valor_nutrimental: 3, tipo: 'vegetable' },
  { id: 35, nombre: 'Chocolate', valor_nutrimental: 4, tipo: 'proteina' },
  { id: 36, nombre: 'Fresa', valor_nutrimental: 3, tipo: 'vegetable' },
  { id: 37, nombre: 'Romero', valor_nutrimental: 3, tipo: 'vegetable' },
  { id: 38, nombre: 'Mantequilla', valor_nutrimental: 7, tipo: 'side' },
  { id: 39, nombre: 'Azúcar', valor_nutrimental: 6, tipo: 'side' },
  { id: 40, nombre: 'Harina', valor_nutrimental: 8, tipo: 'proteina' },
  { id: 41, nombre: 'Rodillo', valor_nutrimental: 6, tipo: 'utils' },
  { id: 42, nombre: 'Moldes', valor_nutrimental: 7, tipo: 'utils' },
  { id: 43, nombre: 'Fondant blanco', valor_nutrimental: 9, tipo: 'vegetable' },
  { id: 44, nombre: 'Mermelada de frambuesa', valor_nutrimental: 7, tipo: 'vegetable' },
  { id: 45, nombre: 'Aceitunas', valor_nutrimental: 4, tipo: 'side' },
  { id: 46, nombre: 'Ensalada', valor_nutrimental: 3, tipo: 'vegetable' }
];

const recetas = [
  // Picnic
  {
    es_principal: true,
    ingredientes: {
      side: {
        ingrediente1: 'Aceite de oliva',
        ingrediente2: 'Sal',
      },
      verduras: {
        ingrediente1: 'Cebolla',
        ingrediente2: 'Tocino',
      },
      protein: {
        ingrediente1: 'Huevo',
        ingrediente2: 'Jamon',
      },
      utils: {
        ingrediente1: 'Cuchillo',
        ingrediente2: 'Boul',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Pan integral',
      },
      verduras: {
        ingrediente1: 'Queso feta',
        ingrediente2: 'Tomates cherry',
      },
      protein: {
        ingrediente1: 'Pollo',
      },
      utils: {
        ingrediente1: 'Pinzas',
        ingrediente2: 'Parrilla',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Aceitunas negras',
        ingrediente2: 'Palta omega',
      },
      verduras: {
        ingrediente1: 'Tomates cherry',
        ingrediente2: 'Ensalada',
      },
      protein: {
        ingrediente1: 'Pollo',
      },
      utils: {
        ingrediente1: 'Sarten',
        ingrediente2: 'Pinzas',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Aceitunas negras',
      },
      verduras: {
        ingrediente1: 'Ensalada',
        ingrediente2: 'Queso feta',
      },
      protein: {
        ingrediente1: 'Queso',
      },
      utils: {
        ingrediente1: 'Parrilla',
        ingrediente2: 'Sarten',
      },
    },
  },
  // Cena de Navidad
  {
    es_principal: true,
    ingredientes: {
      side: {
        ingrediente1: 'Mostaza',
        ingrediente2: 'Sidra de manzana',
      },
      verduras: {
        ingrediente1: 'Romero',
        ingrediente2: 'Cebolla',
      },
      protein: {
        ingrediente1: 'Cerdo',
      },
      utils: {
        ingrediente1: 'Tabla de cortar',
        ingrediente2: 'Termometro de carne',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Pure de papas',
        ingrediente2: 'Salsa arandanos',
      },
      verduras: {
        ingrediente1: 'Zanahorias Glaseadas',
        ingrediente2: 'Coles de bruselas',
      },
      protein: {
        ingrediente1: 'Pavo',
      },
      utils: {
        ingrediente1: 'Cuchara de madera',
        ingrediente2: 'Pinzas',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Arroz',
        ingrediente2: 'Salsa de arandanos',
      },
      verduras: {
        ingrediente1: 'Ensalada',
        ingrediente2: 'Esparragos',
      },
      protein: {
        ingrediente1: 'Jamon',
      },
      utils: {
        ingrediente1: 'Pinzas',
        ingrediente2: 'Olla',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Arroz',
        ingrediente2: 'Arroz de arandanos',
      },
      verduras: {
        ingrediente1: 'Ensalada',
        ingrediente2: 'Esparragos',
      },
      protein: {
        ingrediente1: 'Pavo',
      },
      utils: {
        ingrediente1: 'Pinzas',
        ingrediente2: 'Olla',
      },
    },
  },
  // Boda
  {
    es_principal: true,
    ingredientes: {
      side: {
        ingrediente1: 'Mantequilla',
        ingrediente2: 'Azucar blanca',
      },
      verduras: {
        ingrediente1: 'Fondant blanco',
        ingrediente2: 'Mermelada de frambuesa',
      },
      protein: {
        ingrediente1: 'Harina',
        ingrediente2: 'Huevos',
      },
      utils: {
        ingrediente1: 'Rodillo',
        ingrediente2: 'Moldes',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Limon',
        ingrediente2: 'Crema de leche',
      },
      verduras: {
        ingrediente1: 'Romero',
        ingrediente2: 'Ensalada',
      },
      protein: {
        ingrediente1: 'Salmon',
      },
      utils: {
        ingrediente1: 'Sarten',
        ingrediente2: 'Batidor',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Papas',
        ingrediente2: 'Limon',
      },
      verduras: {
        ingrediente1: 'Fresas',
        ingrediente2: 'Ensalada',
      },
      protein: {
        ingrediente1: 'Salmon',
      },
      utils: {
        ingrediente1: 'Horno',
        ingrediente2: 'Sarten',
      },
    },
  },
  {
    es_principal: false,
    ingredientes: {
      side: {
        ingrediente1: 'Crema de leche',
      },
      verduras: {
        ingrediente1: 'Fresas',
        ingrediente2: 'Salsa de arándanos',
      },
      protein: {
        ingrediente1: 'Chocolate Oscuro',
      },
      utils: {
        ingrediente1: 'Batidora',
        ingrediente2: 'Cuchara de madera',
      },
    },
  },
];

async function populateDatabase() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Eliminar todas las cartas y recetas existentes utilizando CASCADE
    await sequelize.query('TRUNCATE "Carta" CASCADE');
    await sequelize.query('TRUNCATE "Receta" CASCADE');

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
