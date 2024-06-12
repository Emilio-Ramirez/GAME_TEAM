const { sequelize } = require('./db');

async function syncAndDropTables() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Eliminar todas las tablas de la base de datos
    await sequelize.dropAllSchemas();
    console.log('All tables dropped successfully');

    // Sincronizar los modelos con la base de datos
    await sequelize.sync({ force: true });
    console.log('Tables synced with the models');

    await sequelize.close();
    console.log('Database connection closed');
    process.exit(0);
  } catch (error) {
    console.error('Error syncing tables:', error);
    process.exit(1);
  }
}

syncAndDropTables();
