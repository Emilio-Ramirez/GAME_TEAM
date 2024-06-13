// public/js/renderData.js
const Chart = require('chart.js/auto');

fetch('/api/usuarios-por-rango')
  .then(response => response.json())
  .then(data => {
    renderUsuariosPorRango(data);
    renderGraficaPastel(data);
  })
  .catch(error => {
    console.error('Error obteniendo los datos:', error);
  });


fetch('/api/estadistica-usuarios-semana')
  .then(response => response.json())
  .then(data => {
    renderUsuariosSemana(data);
  })
  .catch(error => {
    console.error('Error obteniendo los datos:', error);
  });

fetch('/api/estadistica-partidas-semana')
  .then(response => response.json())
  .then(data => {
    renderPartidasSemana(data);
  })
  .catch(error => {
    console.error('Error obteniendo los datos:', error);
  });

fetch('/api/top-puntajes')
  .then(response => response.json())
  .then(data => {
    renderTopPuntajes(data);
  })
  .catch(error => {
    console.error('Error obteniendo los datos:', error);
  });

fetch('/api/estadistica-promedio-puntaje-nivel')
  .then(response => response.json())
  .then(data => {
    renderPromedioPuntajeNivel(data);
  })
  .catch(error => {
    console.error('Error obteniendo los datos:', error);
  });


function renderGraficaPastel(data) {
  const labels = data.map(item => item.usr_rank);
  const valores = data.map(item => item.cantidad_usuarios);

  const ctx = document.getElementById('grafica-pastel').getContext('2d');
  new Chart(ctx, {
    type: 'pie',
    data: {
      labels: labels,
      datasets: [{
        label: 'Usuarios por rango',
        data: valores,
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)', // Rojo
          'rgba(54, 162, 235, 0.2)', // Azul
          'rgba(255, 206, 86, 0.2)'  // Amarillo
        ],
        borderColor: [
          'rgba(255, 99, 132, 1)', // Rojo
          'rgba(54, 162, 235, 1)', // Azul
          'rgba(255, 206, 86, 1)'  // Amarillo
        ],
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });
}


function renderUsuariosPorRango(data) {
  const contentElement = document.querySelector('.content');

  // Crea un elemento <h2> para el título
  const tituloElement = document.createElement('h2');
  tituloElement.textContent = 'Usuarios por rango';
  contentElement.appendChild(tituloElement);

  // Crea un elemento <ul> para la lista
  const listaElement = document.createElement('ul');

  // Itera sobre los datos y crea un elemento <li> para cada rango
  data.forEach(item => {
    const itemElement = document.createElement('li');
    itemElement.textContent = `${item.usr_rank}: ${item.cantidad_usuarios}`;
    listaElement.appendChild(itemElement);
  });

  // Agrega la lista al contenido
  contentElement.appendChild(listaElement);
}

function renderUsuariosSemana(data) {
  const ctx = document.getElementById('grafica-usuarios-semana').getContext('2d');

  // Obtener las etiquetas y los valores de la respuesta JSON
  const labels = data.map(item => `Semana ${item.semana}`);
  const valores = data.map(item => parseInt(item.cantidad_usuarios));

  new Chart(ctx, {
    type: 'bar',
    data: {
      labels: labels,
      datasets: [{
        label: 'Cantidad de Usuarios',
        data: valores,
        backgroundColor: 'rgba(75, 192, 192, 0.6)',
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          precision: 0
        }
      },
      plugins: {
        legend: {
          display: false
        },
        title: {
          display: true,
          text: 'Cantidad de Usuarios por Semana'
        }
      }
    }
  });
}

function renderPartidasSemana(data) {
  const ctx = document.getElementById('grafica-partidas-semana').getContext('2d');

  // Obtener las etiquetas y los valores de la respuesta JSON
  const labels = data.map(item => item.dia);
  const valores = data.map(item => parseInt(item.cantidad_conexiones));

  new Chart(ctx, {
    type: 'bar',
    data: {
      labels: labels,
      datasets: [{
        label: 'Cantidad de Partidas',
        data: valores,
        backgroundColor: 'rgba(255, 99, 132, 0.6)',
        borderColor: 'rgba(255, 99, 132, 1)',
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          precision: 0
        }
      },
      plugins: {
        legend: {
          display: false
        },
        title: {
          display: true,
          text: 'Cantidad de Partidas por Día'
        }
      }
    }
  });
}

function renderTopPuntajes(data) {
  const ctx = document.getElementById('grafica-top-puntajes').getContext('2d');

  // Obtener las etiquetas y los valores de la respuesta JSON
  const labels = data.map(item => item.username);
  const valores = data.map(item => item.puntaje_maximo);

  new Chart(ctx, {
    type: 'line',
    data: {
      labels: labels,
      datasets: [{
        label: 'Puntaje Máximo',
        data: valores,
        backgroundColor: 'rgba(54, 162, 235, 0.6)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 2,
        fill: false,
        tension: 0.1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true,
          precision: 0
        }
      },
      plugins: {
        legend: {
          display: false
        },
        title: {
          display: true,
          text: 'Top Puntajes'
        }
      }
    }
  });
}

function renderPromedioPuntajeNivel(data) {
  // Implementa la lógica para renderizar la gráfica de promedio de puntaje por nivel
  // Utiliza el elemento <canvas> con el ID "grafica-promedio-puntaje-nivel"
}

