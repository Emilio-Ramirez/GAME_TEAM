const { sequelize } = require('./db');

sequelize.sync()
  .then(() => {
    console.log('Tables synced with the models');
    process.exit(0);
  })
  .catch((error) => {
    console.error('Error syncing tables:', error);
    process.exit(1);
  });
