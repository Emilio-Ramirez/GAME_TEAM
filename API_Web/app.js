const express = require('express');
const { sequelize, Partida, LibroReceta, Receta, SetPlatillos, CartaReceta, Usuario, Evento, Baraja, BarajaCarta, Sesion, Carta } = require('./db');
const bodyParser = require('body-parser');
const app = express();
const bcrypt = require('bcrypt');
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

app.listen(3000, () => {
  console.log('Server is running on http://localhost:3000');
});
