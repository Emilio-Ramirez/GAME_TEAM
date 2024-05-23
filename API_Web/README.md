# Node.js Project

This is a Node.js project with a start command using nodemon.

## Prerequisites

- Node.js (version X.X.X)
- npm (version X.X.X)

## Getting Started

1. Clone the repository:

2. Navigate to the project directory:
3. Install the dependencies:
 ```
 npm install
 ```
5. Start the application:
For runing the application use the following command:
```
npm start
```
5. Migrations:
```
npm run sync_db
```
6. Populating the database:
We have a file that if you run it drop the contents of the tables that are Recetas y Cartas
```
npm run populateDB
```

## Sequelize Usage

This project uses Sequelize, a powerful ORM (Object-Relational Mapping) tool for Node.js, to interact with the database. Sequelize provides an easy and efficient way to define models, establish associations between them, and perform database operations.
The project follows these steps to set up and use Sequelize:

#### Configuration:
    The Sequelize instance is created and configured using the database connection details specified in the environment variables.
#### Model Definition: 
    The project defines various models using Sequelize's define method. Each model represents a table in the database and specifies its attributes and data types.
#### Associations:
    Associations between models are established using Sequelize's association methods, such as belongsTo, hasMany, belongsToMany, etc. These associations define the relationships between different models and enable efficient querying and data retrieval.
#### Database Synchronization: 
    The sync_db script is used to synchronize the defined models with the database. It creates the necessary tables and associations in the database based on the model definitions.
#### Database Operations: 
    Sequelize provides a wide range of methods to perform database operations, such as querying, creating, updating, and deleting records. These methods are used throughout the project to interact with the database and manipulate data.

## License

This project is licensed under the [MIT License](LICENSE).
