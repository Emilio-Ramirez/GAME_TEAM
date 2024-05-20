const express = require('express');
const { sequelize, Partida, LibroReceta, Receta, SetPlatillos, CartaReceta, Usuario, Evento, Baraja, BarajaCarta, Carta } = require('./db');
const bodyParser = require('body-parser');
const app = express();
const bcrypt = require('bcrypt');


// Add these lines before your routes
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// Example route for Partida model
app.get('/partidas', async (req, res) => {
  try {
    const partidas = await Partida.findAll();
    res.json(partidas);
  } catch (error) {
    console.error('Error fetching partidas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for LibroReceta model
app.get('/libros-recetas', async (req, res) => {
  try {
    const librosRecetas = await LibroReceta.findAll();
    res.json(librosRecetas);
  } catch (error) {
    console.error('Error fetching libros de recetas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for Receta model
app.get('/recetas', async (req, res) => {
  try {
    const recetas = await Receta.findAll();
    res.json(recetas);
  } catch (error) {
    console.error('Error fetching recetas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for SetPlatillos model
app.get('/set-platillos', async (req, res) => {
  try {
    const setPlatillos = await SetPlatillos.findAll();
    res.json(setPlatillos);
  } catch (error) {
    console.error('Error fetching set de platillos:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for CartaReceta model
app.get('/carta-recetas', async (req, res) => {
  try {
    const cartaRecetas = await CartaReceta.findAll();
    res.json(cartaRecetas);
  } catch (error) {
    console.error('Error fetching carta de recetas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for Usuario model
app.get('/usuarios', async (req, res) => {
  try {
    const usuarios = await Usuario.findAll();
    res.json(usuarios);
  } catch (error) {
    console.error('Error fetching usuarios:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for Evento model
app.get('/eventos', async (req, res) => {
  try {
    const eventos = await Evento.findAll();
    res.json(eventos);
  } catch (error) {
    console.error('Error fetching eventos:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for Baraja model
app.get('/barajas', async (req, res) => {
  try {
    const barajas = await Baraja.findAll();
    res.json(barajas);
  } catch (error) {
    console.error('Error fetching barajas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for BarajaCarta model
app.get('/baraja-cartas', async (req, res) => {
  try {
    const barajaCartas = await BarajaCarta.findAll();
    res.json(barajaCartas);
  } catch (error) {
    console.error('Error fetching baraja de cartas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example route for Carta model
app.get('/cartas', async (req, res) => {
  try {
    const cartas = await Carta.findAll();
    res.json(cartas);
  } catch (error) {
    console.error('Error fetching cartas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});


// Route for registering a user
app.post('/register', async (req, res) => {
  try {
    const { username, password } = req.body;

    // Check if the username already exists
    const existingUser = await Usuario.findOne({ where: { username } });
    if (existingUser) {
      return res.status(409).json({ error: 'Username already exists' });
    }

    // Hash the password
    const saltRounds = 10;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    // Create a new user
    const newUser = await Usuario.create({ username, password: hashedPassword });

    res.status(201).json({ message: 'User registered successfully' });
  } catch (error) {
    console.error('Error registering user:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Ruta para obtener todas las cartas
app.get('/cartas', async (req, res) => {
  try {
    // Obtener todas las cartas de la base de datos
    const cartas = await Carta.findAll();

    // Enviar las cartas como respuesta
    res.json(cartas);
  } catch (error) {
    console.error('Error al obtener las cartas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

app.listen(3000, () => {
  console.log('Server is running on http://localhost:3000');
});
