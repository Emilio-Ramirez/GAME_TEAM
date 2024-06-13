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

function renderUsuariosPorRango(data) {
  // ... (código de la función)
}

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



