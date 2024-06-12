using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;

    void Start()
    {
        // Asegúrate de que el valor del slider refleje el volumen actual al inicio
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
        }

        // Añadir un listener al slider para llamar al método OnVolumeChange cada vez que cambie el valor del slider
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    public void OnVolumeChange(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
