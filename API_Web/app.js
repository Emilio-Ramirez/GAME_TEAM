const express = require('express');
const { sequelize, Usuario, Receta, Nivel, Partida, Cartas, Sesion } = require('./db');
const bodyParser = require('body-parser');
const app = express();
const bcrypt = require('bcrypt');
const dotenv = require('dotenv');
const { v4: uuidv4 } = require('uuid');
const moment = require('moment')
const { Op } = require('sequelize');
const cors = require('cors');

app.use(cors({
  origin: 'http://localhost:3001'
}));

// Load environment variables
dotenv.config();

// Add these lines before your routes
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

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

// Function to generate unique session tokens
const generateSessionToken = () => {
  return uuidv4();
};

// Middleware to authenticate session
const authenticateSession = async (req, res, next) => {
  try {
    let { token } = req.headers;

    // Parse the token from the JSON string
    let parsedToken = JSON.parse(token);
    let extractedToken = parsedToken.token;

    // Find the session in the database
    console.log('Token:', token);
    console.log('Extracted Token:', extractedToken);
    console.log('Searching token');
    const session = await Sesion.findOne({ where: { token: extractedToken } });
    if (!session) {
      return res.status(401).json({ error: 'Invalid session' });
    }
    console.log('Token found');
    // Check if the session has expired
    if (session.fecha_expiracion < new Date()) {
      await session.destroy();
      return res.status(401).json({ error: 'Session expired' });
    }

    // Update the last activity of the session
    session.ultima_actividad = new Date();
    await session.save();

    req.session = session;
    next();
  } catch (error) {
    console.error('Error authenticating session:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

// Route for login
app.post('/login', async (req, res) => {
  try {
    const { username, password } = req.body;

    // Find the user in the database
    const user = await Usuario.findOne({ where: { username } });
    if (!user) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Verify the password
    const isPasswordValid = await bcrypt.compare(password, user.password);
    if (!isPasswordValid) {
      return res.status(401).json({ error: 'Invalid credentials' });
    }

    // Generate the session token
    const token = generateSessionToken();

    // Create a new session
    const session = await Sesion.create({
      token,
      fecha_inicio: new Date(),
      fecha_expiracion: new Date(Date.now() + 24 * 60 * 60 * 1000), // Expires in 24 hours
      id_usuario: user.id_usuario,
    });

    res.json({ token });
  } catch (error) {
    console.error('Error logging in:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Route for logout
app.post('/logout', async (req, res) => {
  try {
    const { token } = req.body;

    // Find the session in the database
    const session = await Sesion.findOne({ where: { token } });
    if (!session) {
      return res.status(404).json({ error: 'Session not found' });
    }

    // Delete the session
    await session.destroy();

    res.json({ message: 'Logged out successfully' });
  } catch (error) {
    console.error('Error logging out:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Example of a protected route with session authentication
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


// Get route for ingredients with optional filtering by event ID
app.get('/cartas', async (req, res) => {
  try {
    const { eventoId } = req.query;

    // If eventoId is provided, filter cartas by that event ID
    const whereCondition = eventoId ? { eventoId: eventoId } : {};

    const ingredientes = await Cartas.findAll({ where: whereCondition });
    res.json(ingredientes);
  } catch (error) {
    console.error('Error fetching ingredientes:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// Route for updating user scores
app.post('/update-scores', authenticateSession, async (req, res) => {
  try {
    const userId = req.session.id_usuario;
    const { puntaje_maximo, average_dishes_per_event, nivel } = req.body;

    s
    // Validate that the nivel is one of the valid levels
    const validLevels = ['wedding', 'picnic', 'christmas_dinner'];
    if (!validLevels.includes(nivel)) {
      console.log('Invalid level:', nivel);
      return res.status(400).json({ error: 'Invalid level' });
    }

    // Find the user in the database
    const user = await Usuario.findByPk(userId);
    if (!user) {
      console.log('User not found with ID:', userId);
      return res.status(404).json({ error: 'User not found' });
    }


    // Update the user's scores
    user.puntaje_maximo = puntaje_maximo;
    user.average_dishes_per_event = average_dishes_per_event;
    user.usr_rank = nivel; // Update the usr_rank with the nivel

    await user.save();

    console.log('User scores updated successfully');
    console.log('Updated user:', user.toJSON());

    res.json({ message: 'Scores updated successfully' });
  } catch (error) {
    console.error('Error updating scores:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});


app.get('/recetas', async (req, res) => {
  try {
    const { eventoId, id } = req.query; // Obtener ambos parámetros de consulta

    // Construir condición de filtro según los parámetros recibidos
    let whereCondition = {};
    if (eventoId) {
      whereCondition.eventoId = eventoId;
    }
    if (id) {
      whereCondition.id_receta = id;
    }

    const recetas = await Receta.findAll({ where: whereCondition });
    res.json(recetas);
  } catch (error) {
    console.error('Error fetching recetas:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
});

app.listen(process.env.API_PORT, () => {
  console.log(`Server is running on http://localhost:${process.env.API_PORT}`);
});


// ************************ 
// Estadisticas
// ************************

app.get('/estadistica-usuarios-semana', async (req, res) => {
  try {
    const usuariosPorSemana = await Sesion.findAll({
      where: {
        fecha_inicio: {
          [Op.gte]: moment().subtract(70, 'days').toDate()
        }
      },
      attributes: [
        [sequelize.literal('EXTRACT(WEEK FROM fecha_inicio) + 1'), 'semana'], // Calcular el n�mero de semana
        [sequelize.fn('COUNT', sequelize.fn('DISTINCT', sequelize.col('id_usuario'))), 'cantidad_usuarios']
      ],
      group: [sequelize.literal('EXTRACT(WEEK FROM fecha_inicio) + 1')], // Agrupar por semana
      order: [[sequelize.literal('EXTRACT(WEEK FROM fecha_inicio) + 1'), 'DESC']] // Ordenar por semana descendente
    });
    res.json(usuariosPorSemana);
  } catch (error) {
    console.error('Error al obtener la cantidad de usuarios por semana en las �ltimas 10 semanas:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

app.get('/estadistica-partidas-semana', async (req, res) => {
  try {
    const conexionesPorDia = await Sesion.findAll({
      where: {
        fecha_inicio: {
          [Op.gte]: moment().subtract(10, 'days').toDate()
        }
      },
      attributes: [
        [sequelize.fn('DATE', sequelize.col('fecha_inicio')), 'dia'],
        [sequelize.fn('COUNT', sequelize.col('id_usuario')), 'cantidad_conexiones']
      ],
      group: [sequelize.fn('DATE', sequelize.col('fecha_inicio'))],
      order: [['dia', 'DESC']]
    });
    res.json(conexionesPorDia);
  } catch (error) {
    console.error('Error al obtener la cantidad de conexiones por d�a en los �ltimos 10 d�as:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

app.get('/top-puntajes', async (req, res) => {
  try {
    const topUsuarios = await Usuario.findAll({
      attributes: ['username', 'puntaje_maximo'],
      where: {
        puntaje_maximo: {
          [Op.not]: null
        }
      },
      order: [['puntaje_maximo', 'DESC']],
      limit: 10,
    });
    res.json(topUsuarios);
  } catch (error) {
    console.error('Error al obtener los usuarios con los puntajes más altos:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});


//Usa la vista v_partidas_por_nivel
app.get('/estadistica-promedio-puntaje-nivel', async (req, res) => {
  try {
    const promedioPuntajeNivel = await sequelize.query(
      'SELECT titulo, AVG(puntaje) AS promedio_puntaje FROM v_partidas_por_nivel GROUP BY titulo',
      { type: sequelize.QueryTypes.SELECT }
    );
    res.json(promedioPuntajeNivel);
  } catch (error) {
    console.error('Error al obtener el promedio de puntaje por nivel:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});


app.get('/estadistica-usuarios-rango', async (req, res) => {
  try {
    const usuariosPorRango = await Usuario.findAll({
      attributes: ['usr_rank', [sequelize.fn('COUNT', sequelize.col('id_usuario')), 'cantidad_usuarios']],
      group: ['usr_rank']
    });

    const usuariosPorRangoFiltrados = usuariosPorRango.filter(usuario => usuario.usr_rank !== null);

    if (usuariosPorRangoFiltrados.length === 0) {
      return res.status(404).json({ mensaje: 'No hay usuarios registrados' });
    }

    res.json(usuariosPorRangoFiltrados);
  } catch (error) {
    console.error('Error al obtener la cantidad de usuarios por rango:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

module.exports = app;
