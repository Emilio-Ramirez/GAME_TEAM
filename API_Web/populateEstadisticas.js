const { sequelize, Usuario, Partida, Sesion } = require('./db');
const moment = require('moment');

const usuarios = [
  { username: 'usuario1', password: 'contrase–a1', puntaje_maximo: 100, usr_rank: 'novato', average_dishes_per_event: 2.5 },
  { username: 'usuario2', password: 'contrase–a2', puntaje_maximo: 200, usr_rank: 'intermedio', average_dishes_per_event: 3.2 },
  { username: 'usuario3', password: 'contrase–a3', puntaje_maximo: 150, usr_rank: 'novato', average_dishes_per_event: 1.8 },
  { username: 'usuario4', password: 'contrase–a4', puntaje_maximo: 300, usr_rank: 'avanzado', average_dishes_per_event: 4.1 },
  { username: 'usuario5', password: 'contrase–a5', puntaje_maximo: 250, usr_rank: 'intermedio', average_dishes_per_event: 3.7 },
  { username: 'usuario6', password: 'contrase–a6', puntaje_maximo: 180, usr_rank: 'novato', average_dishes_per_event: 2.2 },
  { username: 'usuario7', password: 'contrase–a7', puntaje_maximo: 280, usr_rank: 'avanzado', average_dishes_per_event: 3.9 },
  { username: 'usuario8', password: 'contrase–a8', puntaje_maximo: 220, usr_rank: 'intermedio', average_dishes_per_event: 3.1 },
  { username: 'usuario9', password: 'contrase–a9', puntaje_maximo: 130, usr_rank: 'novato', average_dishes_per_event: 1.6 },
  { username: 'usuario10', password: 'contrase–a10', puntaje_maximo: 320, usr_rank: 'avanzado', average_dishes_per_event: 4.4 },
  { username: 'usuario11', password: 'contrase–a11', puntaje_maximo: 190, usr_rank: 'novato', average_dishes_per_event: 2.7 },
  { username: 'usuario12', password: 'contrase–a12', puntaje_maximo: 240, usr_rank: 'intermedio', average_dishes_per_event: 3.5 },
  { username: 'usuario13', password: 'contrase–a13', puntaje_maximo: 160, usr_rank: 'novato', average_dishes_per_event: 2.0 },
  { username: 'usuario14', password: 'contrase–a14', puntaje_maximo: 290, usr_rank: 'avanzado', average_dishes_per_event: 4.2 },
  { username: 'usuario15', password: 'contrase–a15', puntaje_maximo: 270, usr_rank: 'intermedio', average_dishes_per_event: 3.8 }
];

async function generarPartidasAleatorias(usuarioId) {
  const partidas = [];
  const fechaActual = moment();

  for (let i = 0; i < 20; i++) {
    const fecha = fechaActual.subtract(Math.random() * 30, 'days').toDate();
    const puntaje = Math.floor(Math.random() * 200) + 50;

    partidas.push({
      fecha,
      puntaje,
      id_usuario: usuarioId
    });
  }

  await Partida.bulkCreate(partidas);
}

async function generarSesionesAleatorias(usuarioId) {
  const sesiones = [];
  const fechaActual = moment();

  for (let i = 0; i < 10; i++) {
    const fechaInicio = fechaActual.subtract(Math.random() * 7, 'days').toDate();
    const fechaExpiracion = moment(fechaInicio).add(Math.random() * 3, 'days').toDate();
    const ultimaActividad = moment(fechaInicio).add(Math.random() * 72, 'hours').toDate();

    sesiones.push({
      token: `token-${usuarioId}-${i}`,
      fecha_inicio: fechaInicio,
      fecha_expiracion: fechaExpiracion,
      ultima_actividad: ultimaActividad,
      id_usuario: usuarioId
    });
  }

  await Sesion.bulkCreate(sesiones);
}

async function populateEstadisticas() {
  try {
    await sequelize.authenticate();
    console.log('Conexi—n a la base de datos establecida');

    await sequelize.query('TRUNCATE TABLE "Usuario" CASCADE');
    await sequelize.query('ALTER SEQUENCE "Usuario_id_usuario_seq" RESTART WITH 1');

    const usuariosCreados = await Usuario.bulkCreate(usuarios);
    console.log('Usuarios creados exitosamente');

    for (const usuario of usuariosCreados) {
      await generarPartidasAleatorias(usuario.id_usuario);
      await generarSesionesAleatorias(usuario.id_usuario);
    }

    console.log('Partidas y sesiones generadas exitosamente');

    await sequelize.close();
    console.log('Conexi—n a la base de datos cerrada');
  } catch (error) {
    console.error('Error al conectarse a la base de datos:', error);
  }
}

populateEstadisticas();
