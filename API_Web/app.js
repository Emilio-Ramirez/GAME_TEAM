const express = require('express');
const { sequelize, Partida, LibroReceta, Receta, SetPlatillos, CartaReceta, Usuario, Evento, Baraja, BarajaCarta, Sesion, Carta } = require('./db');
const bodyParser = require('body-parser');
const app = express();
const bcrypt = require('bcrypt');
const dotenv = require('dotenv');
const { v4: uuidv4 } = require('uuid');


// Add these lines before your routes
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());


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


// Función para generar tokens de sesión únicos
const generateSessionToken = () => {
  return uuidv4();
};

// Middleware para verificar la sesión
const authenticateSession = async (req, res, next) => {
  try {
    const { token } = req.headers;

    // Buscar la sesión en la base de datos
    const session = await Sesion.findOne({ where: { token } });
    if (!session) {
      return res.status(401).json({ error: 'Invalid session' });
    }

    // Verificar si la sesión ha expirado
    if (session.fecha_expiracion < new Date()) {
      await session.destroy();
      return res.status(401).json({ error: 'Session expired' });
    }

    // Actualizar la última actividad de la sesión
    session.ultima_actividad = new Date();
    await session.save();

    req.session = session;
    next();
  } catch (error) {
    console.error('Error authenticating session:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

// Ruta para iniciar sesión
app.post('/login', async (req, res) => {
  try {
    const { username, password } = req.body;

    // Buscar el usuario en la base de datos
    const user = await Usuario.findOne({ where: { username } });
    if (!user) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Verificar la contraseña
    const isPasswordValid = await bcrypt.compare(password, user.password);
    if (!isPasswordValid) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Generar el token de sesión
    const token = generateSessionToken();

    // Crear una nueva sesión
    const session = await Sesion.create({
      token,
      fecha_inicio: new Date(),
      fecha_expiracion: new Date(Date.now() + 24 * 60 * 60 * 1000), // Expira en 24 horas
      id_usuario: user.id_usuario,
    });

    res.json({ token });
  } catch (error) {
    console.error('Error logging in:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Ruta para cerrar sesión
app.post('/logout', async (req, res) => {
  try {
    const { token } = req.body;

    // Buscar la sesión en la base de datos
    const session = await Sesion.findOne({ where: { token } });
    if (!session) {
      return res.status(404).json({ error: 'Session not found' });
    }

    // Eliminar la sesión
    await session.destroy();

    res.json({ message: 'Logged out successfully' });
  } catch (error) {
    console.error('Error logging out:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Ejemplo de ruta protegida por autenticación de sesión
app.get('/profile', authenticateSession, async (req, res) => {
  try {
    const userId = req.session.id_usuario;
    const user = await Usuario.findByPk(userId);
    res.json(user);
  } catch (error) {
    console.error('Error fetching profile:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

//get to cartas
app.get('/cartas', async (req, res) => {
  try {
    const cartas = await Carta.findAll();
    res.json(cartas);
  } catch (error) {
    console.error('Error fetching cartas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}
)

// Ruta para actualizar los puntajes de un usuario
app.post('/update-scores', authenticateSession, async (req, res) => {
  try {
    const userId = req.session.id_usuario;
    const { puntaje_maximo, dishes_per_event, nivel } = req.body;

    console.log('Updating scores for user ID:', userId);
    console.log('Received scores:', { puntaje_maximo, dishes_per_event, nivel });

    // Validar que el nivel sea uno de los tres niveles válidos
    const validLevels = ['wedding', 'picnic', 'christmas_dinner'];
    if (!validLevels.includes(nivel)) {
      console.log('Invalid level:', nivel);
      return res.status(400).json({ error: 'Invalid level' });
    }

    // Buscar el usuario en la base de datos
    const user = await Usuario.findByPk(userId);
    if (!user) {
      console.log('User not found with ID:', userId);
      return res.status(404).json({ error: 'User not found' });
    }

    console.log('User found:', user.toJSON());

    // Verificar si ya existe un registro de puntaje para el nivel especificado
    const existingScore = user.puntajes.find((score) => score.nivel === nivel);

    if (existingScore) {
      console.log('Existing score found for level:', nivel);
      console.log('Updating existing score:', existingScore);
      // Actualizar los puntajes existentes para el nivel
      existingScore.puntaje_maximo = puntaje_maximo;
      existingScore.dishes_per_event = dishes_per_event;
    } else {
      console.log('No existing score found for level:', nivel);
      console.log('Adding new score');
      // Agregar un nuevo registro de puntaje para el nivel
      user.puntajes.push({
        puntaje_maximo,
        dishes_per_event,
        nivel,
      });
    }

    // Update the user's puntajes field in the database
    await Usuario.update(
      { puntajes: user.puntajes },
      { where: { id_usuario: userId } }
    );

    console.log('User scores updated successfully');

    const updatedUser = await Usuario.findByPk(userId);
    console.log('Updated user:', updatedUser.toJSON());

    res.json({ message: 'Scores updated successfully' });
  } catch (error) {
    console.error('Error updating scores:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

app.get('/recetas', async (req, res) => {
  try {
    const recetas = await Receta.findAll();
    res.json(recetas);
  } catch (error) {
    console.error('Error fetching recetas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}
)



app.listen(process.env.API_PORT, () => {
  console.log('Server is running on http://localhost:' + process.env.API_PORT)
});
