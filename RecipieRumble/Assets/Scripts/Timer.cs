using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float totalTime = 60f;  // Tiempo total para el juego en segundos (1 minuto)
    private float currentTime;  // Tiempo actual restante
    private bool isTimerRunning = false;
    public delegate void TimeUpAction();
    public static event TimeUpAction OnTimeUp;  // Evento desencadenado cuando se acaba el tiempo
    public GameObject gameOverScreenPrefab;  // Referencia al prefab de la pantalla de Game Over
    public Canvas mainCanvas;  // Referencia al Canvas principal

    public TMP_Text timerText;
    public Button startButton;  // Referencia al botón de UI para iniciar el temporizador
    public AudioSource audioSource;  // Referencia al AudioSource para reproducir el sonido


    void Start()
    {
        // Asegúrate de que el temporizador no esté corriendo al inicio
        isTimerRunning = false;

        // Inicializa currentTime con totalTime
        currentTime = totalTime;

        // Opcional: Inicializa el texto del temporizador
        UpdateTimerText();

        // Asigna el método StartTimer al evento onClick del botón
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartTimer);
        }

        // Suscribirse al evento de cambio de escena
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // Mensajes de depuración
        Debug.Log("Start() - totalTime: " + totalTime + ", currentTime: " + currentTime);
    }

    void Update()
    {
        // Actualiza el temporizador si está corriendo
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;  // Reduce el tiempo actual por deltaTime en cada frame
            if (currentTime <= 0)
            {
                // El temporizador ha terminado
                currentTime = 0;
                isTimerRunning = false;
                Debug.Log("¡Se acabó el tiempo!");

                // Detener el audio
                StopAudio();

                if (OnTimeUp != null)
                {
                    OnTimeUp();  // Desencadena el evento cuando se acaba el tiempo
                }
                // Instancia y muestra la pantalla de Game Over
                if (gameOverScreenPrefab != null)
                {
                    Debug.Log("Instanciando pantalla de Game Over");
                    GameObject gameOverScreen = Instantiate(gameOverScreenPrefab, mainCanvas.transform);
                    Debug.Log("Pantalla de Game Over instanciada en el Canvas");
                }
                else
                {
                    Debug.LogWarning("gameOverScreenPrefab no está asignado en el inspector.");
                }
            }

            UpdateTimerText();  // Actualiza el componente de texto UI con el tiempo actual
        }
    }

    void UpdateTimerText()
    {
        // Actualiza el componente de texto UI con el tiempo actual
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        // Inicia el temporizador
        isTimerRunning = true;
        currentTime = totalTime;

        // Oculta el botón de inicio
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }

        // Reproduce el sonido de inicio si hay un AudioSource y un AudioClip asignados
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Mensaje de depuración
        Debug.Log("StartTimer() - totalTime: " + totalTime + ", currentTime: " + currentTime);
    }

    public void StopTimer()
    {
        // Detiene el temporizador
        isTimerRunning = false;
    }

    public float GetCurrentTime()
    {
        // Obtiene el tiempo actual restante
        return currentTime;
    }

    public bool IsTimerRunning()
    {
        // Comprueba si el temporizador está corriendo
        return isTimerRunning;
    }

    void OnDestroy()
    {
        // Asegurarse de desuscribirse del evento
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene current)
    {
        // Detener el audio cuando la escena se descarga
        StopAudio();
    }

    public void IncreaseTimer()
    {
        totalTime += 15;
        currentTime += 15;
        UpdateTimerText();  // Asegúrate de que el texto del temporizador se actualice también
    }


    private void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
