const { Sequelize, DataTypes } = require('sequelize')
const dotenv = require('dotenv');

// Load environment variables
dotenv.config();

// Create a new Sequelize instance
const sequelize = new Sequelize(process.env.DB_DATABASE, process.env.DB_USERNAME, process.env.DB_PASSWORD, {
  host: process.env.DB_HOST,
  port: process.env.DB_PORT,
  dialect: 'postgres',
});

const Usuario = sequelize.define('Usuario', {
  id_usuario: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  puntaje_maximo: {
    type: DataTypes.INTEGER,
  },
  usr_rank: {
    type: DataTypes.STRING,
  },
  average_dishes_per_event: {
    type: DataTypes.FLOAT,
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
});

const Receta = sequelize.define('Receta', {
  id_receta: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  es_principal: {
    type: DataTypes.BOOLEAN,
    defaultValue: false,
  },
  belongs_to_level: {
    type: DataTypes.INTEGER,
  },
  ingredientes: {
    type: DataTypes.JSON,
    defaultValue: {
      side: {},
      verduras: {},
      protein: {},
      utils: {},
    },
  },
});

const Nivel = sequelize.define('Nivel', {
  id_nivel: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
  },
  titulo: {
    type: DataTypes.STRING,
  },
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

const Cartas = sequelize.define('Cartas', {
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
  id_usuario: {
    type: DataTypes.INTEGER,
    references: {
      model: Usuario,
      key: 'id_usuario',
    },
  },
});

// Export the Sequelize instance and models
module.exports = {
  sequelize,
  Usuario,
  Receta,
  Nivel,
  Partida,
  Cartas,
  Sesion,
};
