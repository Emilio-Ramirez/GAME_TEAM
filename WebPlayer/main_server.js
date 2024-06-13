const express = require('express');
const path = require('path');
const bodyParser = require('body-parser');
const app = express();

// Load environment variables
app.use(express.static(path.join(__dirname, 'public')));
app.use('/Build', express.static(path.join(__dirname, 'public', 'RecipeWeb', 'Build')));

app.use((req, res, next) => {
  if (req.path.endsWith('.js')) {
    res.setHeader('Content-Type', 'application/javascript');
  }
  next();
});

app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'index.html'));
});

app.get('/play', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'play.html'));
});

app.get('/stats', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'stats.html'));
});

app.get('/us', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'us.html'));
});

app.get('/instructions', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'instructions.html'));
});

// ***********************
//          API
// ***********************

// Configurar CORS si es necesario
app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept');
  next();
});

// Asumiendo que el servidor Node.js se ejecuta en localhost:3000
const apiBaseUrl = 'http://localhost:3000';

// Función para realizar una solicitud GET a una URL
async function fetchData(url) {
  try {
    const response = await fetch(url);
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching data:', error);
    throw error;
  }
}


// Nueva ruta para obtener la cantidad de usuarios por rango
app.get('/api/usuarios-por-rango', async (req, res) => {
  try {
    const data = await fetchData(`${apiBaseUrl}/estadistica-usuarios-rango`);
    res.json(data);
  } catch (error) {
    console.error('Error obteniendo la cantidad de usuarios por rango:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

// Nueva ruta para obtener la cantidad de usuarios por semana
app.get('/api/estadistica-usuarios-semana', async (req, res) => {
  try {
    const data = await fetchData(`${apiBaseUrl}/estadistica-usuarios-semana`);
    res.json(data);
  } catch (error) {
    console.error('Error obteniendo la cantidad de usuarios por semana:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

// Nueva ruta para obtener la cantidad de partidas por semana
app.get('/api/estadistica-partidas-semana', async (req, res) => {
  try {
    const data = await fetchData(`${apiBaseUrl}/estadistica-partidas-semana`);
    res.json(data);
  } catch (error) {
    console.error('Error obteniendo la cantidad de partidas por semana:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

// Nueva ruta para obtener los top puntajes
app.get('/api/top-puntajes', async (req, res) => {
  try {
    const data = await fetchData(`${apiBaseUrl}/top-puntajes`);
    res.json(data);
  } catch (error) {
    console.error('Error obteniendo los top puntajes:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

// Nueva ruta para obtener el promedio de puntaje por nivel
app.get('/api/estadistica-promedio-puntaje-nivel', async (req, res) => {
  try {
    const data = await fetchData(`${apiBaseUrl}/estadistica-promedio-puntaje-nivel`);
    res.json(data);
  } catch (error) {
    console.error('Error obteniendo el promedio de puntaje por nivel:', error);
    res.status(500).json({ error: 'Error interno del servidor' });
  }
});

const port = process.env.API_PORT || 3001;
app.listen(port, () => {
  console.log(`Server is running on http://localhost:${port}`);
});
