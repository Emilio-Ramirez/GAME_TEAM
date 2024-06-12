const { sequelize } = require('./db');

async function syncAndDropTables() {
  try {
    await sequelize.authenticate();
    console.log('Database connection established');

    // Obtener los nombres de todas las tablas en la base de datos
    const [results, metadata] = await sequelize.query(`
      SELECT table_name
      FROM information_schema.tables
      WHERE table_schema = 'public'
      AND table_type = 'BASE TABLE';
    `);

    const tableNames = results.map(result => result.table_name);

    // Eliminar todas las tablas de la base de datos
    for (const tableName of tableNames) {
      await sequelize.query(`DROP TABLE IF EXISTS "${tableName}" CASCADE;`);
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
