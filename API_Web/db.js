const { Sequelize, DataTypes } = require('sequelize');
const dotenv = require('dotenv');

// Load environment variables
dotenv.config();

// Create a new Sequelize instance
const sequelize = new Sequelize(process.env.DB_DATABASE, process.env.DB_USERNAME, process.env.DB_PASSWORD, {
  host: process.env.DB_HOST,
  port: process.env.DB_PORT,
  dialect: 'mysql',
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
}, {
  tableName: 'Usuario', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
}, {
  tableName: 'Receta', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
}, {
  tableName: 'Nivel', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
}, {
  tableName: 'Partida', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
}, {
  tableName: 'Cartas', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
}, {
  tableName: 'Sesion', // Especificar expl’citamente el nombre de la tabla
  timestamps: false, // Deshabilitar las columnas createdAt y updatedAt si no las necesitas
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
