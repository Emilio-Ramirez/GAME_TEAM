import express from 'express';
import { fileURLToPath } from 'url';
import { dirname } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const app = express();
app.use(express.static(__dirname + '/public'));

// Función GET para la pagina de inicio
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/public/index.html');
});

//Función get para el player
app.get('/play', (req, res) => {
    res.sendFile(__dirname + '/public/play.html');
});

//Función GET para la pagina de documentación
app.get('/about', (req, res) => {
    res.sendFile(__dirname + '/public/about.html');
});

//Función GET para la pagina us
app.get('/us', (req, res) => {
    res.sendFile(__dirname + '/public/us.html');
});


const port = 3000;

app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});