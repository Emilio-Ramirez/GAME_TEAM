const { Sequelize, DataTypes } = require('sequelize');
const dotenv = require('dotenv');

// Load environment variables
dotenv.config();

// Create a new Sequelize instance
const sequelize = new Sequelize(process.env.DB_DATABASE, process.env.DB_USERNAME, process.env.DB_PASSWORD, {
  host: process.env.DB_HOST,
  port: process.env.DB_PORT,
  dialect: 'postgres',
});

const Partida = sequelize.define('Partida', {
  id_partida: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  fecha: {
    type: DataTypes.DATE,
  },
  puntaje: {
    type: DataTypes.INTEGER,
  },
});

const LibroReceta = sequelize.define('LibroReceta', {
  id_libro: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  titulo: {
    type: DataTypes.STRING,
  },
  numero_recetas: {
    type: DataTypes.INTEGER,
  },
  desbloqueadas: {
    type: DataTypes.BOOLEAN,
  },
});

const Receta = sequelize.define('Receta', {
  id_receta: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  puntaje: {
    type: DataTypes.INTEGER,
  },
  afinidad: {
    type: DataTypes.STRING,
  },
  tiempo: {
    type: DataTypes.INTEGER,
  },
});

const SetPlatillos = sequelize.define('SetPlatillos', {
  id_set: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
});

const CartaReceta = sequelize.define('CartaReceta', {
  // No additional attributes needed for the join table
});

const Usuario = sequelize.define('Usuario', {
  id_usuario: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  username: {
    type: DataTypes.STRING,
    allowNull: false,
    unique: true,
  },
  password: {
    type: DataTypes.STRING,
    allowNull: false,
  },
  puntaje_maximo: {
    type: DataTypes.INTEGER,
  },
  nivel: {
    type: DataTypes.INTEGER,
  },
  dishes_per_event: {
    type: DataTypes.INTEGER,
  },
});

const Evento = sequelize.define('Evento', {
  id_evento: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  titulo: {
    type: DataTypes.STRING,
  },
  caracteristica: {
    type: DataTypes.STRING,
  },
});

const Baraja = sequelize.define('Baraja', {
  id_baraja: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
});

const BarajaCarta = sequelize.define('BarajaCarta', {
  // No additional attributes needed for the join table
});

const Carta = sequelize.define('Carta', {
  id_carta: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  nombre: {
    type: DataTypes.STRING,
  },
  valor_nutrimental: {
    type: DataTypes.INTEGER,
  },
  tipo: {
    type: DataTypes.STRING,
  },
});

const Sesion = sequelize.define('Sesion', {
  id_sesion: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  token: {
    type: DataTypes.STRING,
    unique: true,
  },
  fecha_inicio: {
    type: DataTypes.DATE,
  },
  fecha_expiracion: {
    type: DataTypes.DATE,
  },
  ultima_actividad: {
    type: DataTypes.DATE,
  },
  ip: {
    type: DataTypes.STRING,
  },
  dispositivo: {
    type: DataTypes.STRING,
  },
});


// Define associations

Partida.belongsTo(Usuario, { foreignKey: 'id_usuario' });
Partida.belongsTo(Evento, { foreignKey: 'id_evento' });
Partida.belongsTo(LibroReceta, { foreignKey: 'id_libro' });

LibroReceta.hasMany(Receta, { foreignKey: 'id_libro' });

Receta.belongsToMany(SetPlatillos, { through: 'CartaReceta', foreignKey: 'id_receta' });
SetPlatillos.belongsToMany(Receta, { through: 'CartaReceta', foreignKey: 'id_set' });

SetPlatillos.belongsToMany(Carta, { through: 'CartaReceta', foreignKey: 'id_set' });
Carta.belongsToMany(SetPlatillos, { through: 'CartaReceta', foreignKey: 'id_carta' });

Usuario.hasMany(Partida, { foreignKey: 'id_usuario' });
Usuario.belongsTo(LibroReceta, { foreignKey: 'id_libro' });

Evento.belongsTo(Receta, { foreignKey: 'id_receta' });
Evento.belongsTo(SetPlatillos, { foreignKey: 'id_set' });

Baraja.belongsToMany(Carta, { through: 'BarajaCarta', foreignKey: 'id_baraja' });
Carta.belongsToMany(Baraja, { through: 'BarajaCarta', foreignKey: 'id_carta' });

Usuario.hasMany(Sesion, { foreignKey: 'id_usuario' });
Sesion.belongsTo(Usuario, { foreignKey: 'id_usuario' });
// Export the Sequelize instance and models
module.exports = {
  sequelize,
  Partida,
  LibroReceta,
  Receta,
  SetPlatillos,
  CartaReceta,
  Usuario,
  Evento,
  Baraja,
  BarajaCarta,
  Carta,
  Sesion, // Add the Sesion model here
};
