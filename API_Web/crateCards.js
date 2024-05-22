const { sequelize, Carta } = require('./db');

const cartas = [
  { nombre: 'Pan integral', valor_nutrimental: 4, tipo: 'side' },
  { nombre: 'Pollo', valor_nutrimental: 2, tipo: 'proteina' },
  { nombre: 'Pinzas', valor_nutrimental: 3, tipo: 'utils' },
  { nombre: 'Parrilla', valor_nutrimental: 2, tipo: 'utils' },
  { nombre: 'Queso feta', valor_nutrimental: 3, tipo: 'vegetable' },
  { nombre: 'Tomates cherry', valor_nutrimental: 2, tipo: 'vegetable' },
  { nombre: 'Huevo', valor_nutrimental: 8, tipo: 'proteina' },
  { nombre: 'Jamón', valor_nutrimental: 7, tipo: 'proteina' },
  { nombre: 'Cebolla', valor_nutrimental: 6, tipo: 'vegetable' },
  { nombre: 'Tocino', valor_nutrimental: 7, tipo: 'vegetable' },
  { nombre: 'Cuchillo', valor_nutrimental: 6, tipo: 'utils' },
  { nombre: 'Bowl', valor_nutrimental: 8, tipo: 'utils' },
  { nombre: 'Aceite de oliva', valor_nutrimental: 7, tipo: 'side' },
  { nombre: 'Sal', valor_nutrimental: 5, tipo: 'side' },
  { nombre: 'Pavo', valor_nutrimental: 3, tipo: 'proteina' },
  { nombre: 'Puré de papas', valor_nutrimental: 4, tipo: 'side' },
  { nombre: 'Zanahoria', valor_nutrimental: 4, tipo: 'vegetable' },
  { nombre: 'Coles de Bruselas', valor_nutrimental: 3, tipo: 'vegetable' },
  { nombre: 'Horno', valor_nutrimental: 3, tipo: 'utils' },
  { nombre: 'Olla', valor_nutrimental: 4, tipo: 'utils' },
  { nombre: 'Cuchara de madera', valor_nutrimental: 4, tipo: 'utils' },
  { nombre: 'Arroz', valor_nutrimental: 3, tipo: 'side' },
  { nombre: 'Salsa de arándano', valor_nutrimental: 5, tipo: 'side' },
  { nombre: 'Cerdo', valor_nutrimental: 8, tipo: 'proteina' },
  { nombre: 'Tabla de cortar', valor_nutrimental: 6, tipo: 'utils' },
  { nombre: 'Termómetro', valor_nutrimental: 8, tipo: 'utils' },
  { nombre: 'Mostaza', valor_nutrimental: 6, tipo: 'side' },
  { nombre: 'Sidra de manzana', valor_nutrimental: 7, tipo: 'side' },
  { nombre: 'Salmón', valor_nutrimental: 3, tipo: 'proteina' },
  { nombre: 'Crema de leche', valor_nutrimental: 4, tipo: 'side' },
  { nombre: 'Sartén', valor_nutrimental: 2, tipo: 'utils' },
  { nombre: 'Batidora', valor_nutrimental: 3, tipo: 'utils' },
  { nombre: 'Limón', valor_nutrimental: 2, tipo: 'side' },
  { nombre: 'Espárragos', valor_nutrimental: 3, tipo: 'vegetable' },
  { nombre: 'Chocolate', valor_nutrimental: 4, tipo: 'proteina' },
  { nombre: 'Fresa', valor_nutrimental: 3, tipo: 'vegetable' },
  { nombre: 'Romero', valor_nutrimental: 3, tipo: 'vegetable' },
  { nombre: 'Mantequilla', valor_nutrimental: 7, tipo: 'side' },
  { nombre: 'Azúcar', valor_nutrimental: 6, tipo: 'side' },
  { nombre: 'Harina', valor_nutrimental: 8, tipo: 'proteina' },
  { nombre: 'Rodillo', valor_nutrimental: 6, tipo: 'utils' },
  { nombre: 'Moldes', valor_nutrimental: 7, tipo: 'utils' },
  { nombre: 'Fondant blanco', valor_nutrimental: 9, tipo: 'vegetable' },
  { nombre: 'Mermelada de frambuesa', valor_nutrimental: 7, tipo: 'vegetable' },
  { nombre: 'Aceitunas', valor_nutrimental: 4, tipo: 'side' },
  { nombre: 'Ensalada', valor_nutrimental: 3, tipo: 'vegetable' }
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
