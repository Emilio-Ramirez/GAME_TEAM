// Se define un event listener para que cuando el evento DOM content loaded (un evento para que se parsee todo html) se complete, 
// se ejecute la función.

document.addEventListener("DOMContentLoaded", function() {

    // Definición de los builds

    var buildUrl = "RecipeWeb/Build";
    var loaderUrl = buildUrl + "/RecipeWeb.loader.js";

    // Definición de la configuración del proyecto sin comprimir en un formato de objeto literal para la config

    var config = {
        dataUrl: buildUrl + "/RecipeWeb.data",
        frameworkUrl: buildUrl + "/RecipeWeb.framework.js",
        codeUrl: buildUrl + "/RecipeWeb.wasm",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "Tec De Monterrey",
        productName: "RecipieRumble_",
        productVersion: "2.0",
    };

    // Obtiene el elemento donde se renderizara el juego
    var canvas = document.getElementById('unityCanvas');
    // Crea un nuevo elemento <script> en HTML
    var script = document.createElement("script");
    // .src deine la URL del script que el elemento debe ejecutar, y esta URL es el script generado por Unity
    script.src = loaderUrl;

    // onload refiere a la capacidad de esperar a que el script de loaderURL se haya terminado de cargar
    // Es control de eventos
    script.onload = () => {

        //Se crea y maneja la instancia de manera standar con las funciones y objetos proporcionados por el export de Unity
        createUnityInstance(canvas, config, (progress) => { 
            document.getElementById('progressBarFull').style.width = 100 * progress + "%";
        }).then((unityInstance) => {
            document.getElementById('loadingBar').style.display = "none";
        }).catch((message) => {
            alert(message);
        });
    };

    document.body.appendChild(script);
});