const request = require('supertest');
const app = require('../app'); // Importa tu aplicaci�n Express desde la ra�z del proyecto
const { Cartas } = require('../db'); // Importa el modelo Cartas desde la ra�z del proyecto

describe('GET /cartas', () => {
  let ingredientesEsperados;

  beforeAll(async () => {
    // Obtener todos los ingredientes existentes en la base de datos
    const ingredientes = await Cartas.findAll();

    // Mapear los ingredientes al formato esperado
    ingredientesEsperados = ingredientes.map(ingrediente => {
      return {
        id_carta: ingrediente.id_carta,
        nombre: ingrediente.nombre,
        valor_nutrimental: ingrediente.valor_nutrimental,
        tipo: ingrediente.tipo,
        createdAt: new Date(ingrediente.createdAt),
        updatedAt: new Date(ingrediente.updatedAt),
      };
    });
  });

  it('should return a list of ingredients', async () => {
    const response = await request(app)
      .get('/cartas')
      .expect(200);

    expect(response.body.length).toBe(ingredientesEsperados.length);

    for (let i = 0; i < ingredientesEsperados.length; i++) {
      const ingredienteEsperado = ingredientesEsperados[i];
      const ingredienteRecibido = response.body.find(
        ingrediente => ingrediente.id_carta === ingredienteEsperado.id_carta
      );

      // Convertir las fechas a objetos Date antes de comparar
      ingredienteRecibido.createdAt = new Date(ingredienteRecibido.createdAt);
      ingredienteRecibido.updatedAt = new Date(ingredienteRecibido.updatedAt);

      expect(ingredienteRecibido).toEqual(ingredienteEsperado);
    }
  });
});
