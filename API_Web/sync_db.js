const { sequelize } = require('./db');

async function syncAndDropTables() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Obtener una lista de todas las tablas en la base de datos
    const tables = await sequelize.showAllSchemas();

    // Eliminar todas las tablas
    for (const table of tables) {
      if (table !== 'information_schema') { // Excluir el esquema 'information_schema'
        await sequelize.drop({ tableName: table });
      }
    }
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
