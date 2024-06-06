using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackgroundVideo : MonoBehaviour
{
    public VideoClip videoClip; // Asigna el video clip desde el Inspector
    public Canvas canvas; // Asigna el Canvas desde el Inspector, opcional

    private void Start()
    {
        // Verifica si el Canvas está asignado desde el Inspector, de lo contrario busca en la jerarquía
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }

        if (canvas == null)
        {
            Debug.LogError("No se encontró el Canvas en la escena.");
            return;
        }

        // Crear un GameObject para la RawImage
        GameObject rawImageObject = new GameObject("BackgroundVideo");
        rawImageObject.transform.SetParent(canvas.transform, false);

        // Añadir el componente RawImage
        RawImage rawImage = rawImageObject.AddComponent<RawImage>();

        // Crear y configurar el VideoPlayer
        VideoPlayer videoPlayer = rawImageObject.AddComponent<VideoPlayer>();
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = true;

        // Crear y asignar la RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 0);
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture;

        // Ajustar el tamaño y la posición de la RawImage para que ocupe toda la pantalla
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Configurar la RawImage para que se renderice en el fondo
        CanvasRenderer canvasRenderer = rawImageObject.GetComponent<CanvasRenderer>();
        canvasRenderer.SetAlpha(1.0f); // Asegura que sea totalmente opaco
        rawImageObject.transform.SetAsFirstSibling(); // Asegura que el RawImage esté detrás de otros elementos UI

        // Iniciar la reproducción del video
        videoPlayer.Play();
    }
}
