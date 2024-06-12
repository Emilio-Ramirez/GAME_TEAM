document.addEventListener("DOMContentLoaded", function() {
    var buildUrl = "/Build";
    var loaderUrl = buildUrl + "/RecipeWeb.loader.js";

    var config = {
        dataUrl: buildUrl + "/RecipeWeb.data",
        frameworkUrl: buildUrl + "/RecipeWeb.framework.js",
        codeUrl: buildUrl + "/RecipeWeb.wasm",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "Tec De Monterrey",
        productName: "RecipieRumble_",
        productVersion: "1.0",
    };

    var canvas = document.getElementById('unityCanvas');
    var script = document.createElement("script");
    script.src = loaderUrl;

    script.onload = () => {
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
