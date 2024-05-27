const express = require('express');
const bodyParser = require('body-parser');
const bcrypt = require('bcrypt');
const dotenv = require('dotenv');
const { v4: uuidv4 } = require('uuid');
const pool = require('./db');

dotenv.config();

const app = express();
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// Route for registering a user
app.post('/register', async (req, res) => {
  try {
    const { username, password } = req.body;

    // Check if the username already exists
    const [rows] = await pool.query('SELECT * FROM Usuario WHERE username = ?', [username]);
    if (rows.length > 0) {
      return res.status(409).json({ error: 'Username already exists' });
    }

    // Hash the password
    const saltRounds = 10;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    // Create a new user
    await pool.query('INSERT INTO Usuario (username, password_) VALUES (?, ?)', [username, hashedPassword]);

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
    const [sessions] = await pool.query('SELECT * FROM Sesion WHERE token = ?', [token]);
    const session = sessions[0];
    if (!session) {
      return res.status(401).json({ error: 'Invalid session' });
    }

    // Verificar si la sesión ha expirado
    if (new Date(session.fecha_expiracion) < new Date()) {
      await pool.query('DELETE FROM Sesion WHERE token = ?', [token]);
      return res.status(401).json({ error: 'Session expired' });
    }

    // Actualizar la última actividad de la sesión
    await pool.query('UPDATE Sesion SET ultima_actividad = ? WHERE token = ?', [new Date(), token]);

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
    const [users] = await pool.query('SELECT * FROM Usuario WHERE username = ?', [username]);
    const user = users[0];
    if (!user) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Verificar la contraseña
    const isPasswordValid = await bcrypt.compare(password, user.password_);
    if (!isPasswordValid) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Generar el token de sesión
    const token = generateSessionToken();

    // Crear una nueva sesión
    await pool.query('INSERT INTO Sesion (token, fecha_inicio, fecha_expiracion, id_usuario) VALUES (?, ?, ?, ?)', [
      token,
      new Date(),
      new Date(Date.now() + 24 * 60 * 60 * 1000), // Expira en 24 horas
      user.id_usuario,
    ]);

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
    const [sessions] = await pool.query('SELECT * FROM Sesion WHERE token = ?', [token]);
    const session = sessions[0];
    if (!session) {
      return res.status(404).json({ error: 'Session not found' });
    }

    // Eliminar la sesión
    await pool.query('DELETE FROM Sesion WHERE token = ?', [token]);

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
    const [users] = await pool.query('SELECT * FROM Usuario WHERE id_usuario = ?', [userId]);
    const user = users[0];
    res.json(user);
  } catch (error) {
    console.error('Error fetching profile:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Ruta para obtener cartas
app.get('/cartas', async (req, res) => {
  try {
    const [cartas] = await pool.query('SELECT * FROM Ingrediente');
    res.json(cartas);
  } catch (error) {
    console.error('Error fetching cartas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

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

    // Actualizar los puntajes y estadísticas del usuario
    await pool.query(
      'UPDATE Usuario SET puntaje_maximo = ?, average_dishes_per_event = ?, usr_rank = ? WHERE id_usuario = ?',
      [puntaje_maximo, dishes_per_event, nivel, userId]
    );

    console.log('User scores updated successfully');

    const [updatedUsers] = await pool.query('SELECT * FROM Usuario WHERE id_usuario = ?', [userId]);
    const updatedUser = updatedUsers[0];
    console.log('Updated user:', updatedUser);

    res.json({ message: 'Scores updated successfully' });
  } catch (error) {
    console.error('Error updating scores:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

const port = process.env.API_PORT || 3000;
app.listen(port, '0.0.0.0', () => {
  console.log(`Server is running on http://0.0.0.0:${port}`);
});

