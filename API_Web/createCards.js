const { sequelize, Carta } = require('./db');

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

async function createCards() {
  try {
    await sequelize.authenticate();
    console.log('Conexión a la base de datos establecida');

    // Verificar si las cartas ya existen en la base de datos
    const existingCartas = await Carta.findAll({
      where: {
        nombre: cartas.map(carta => carta.nombre)
      }
    });

    if (existingCartas.length === 0) {
      // Si no existen cartas, insertarlas en la base de datos
      await Carta.bulkCreate(cartas);
      console.log('Cartas insertadas exitosamente');
    } else {
      console.log('Las cartas ya existen en la base de datos');
    }

    await sequelize.close();
    console.log('Conexión a la base de datos cerrada');
  } catch (error) {
    console.error('Error al crear las cartas:', error);
  }
}

createCards();
