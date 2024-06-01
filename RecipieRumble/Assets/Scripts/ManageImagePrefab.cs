using UnityEngine;
using UnityEngine.UI;

public class ManageImagePrefab : MonoBehaviour
{
    public GameObject imagePrefab; // Asigna el prefab de la imagen desde el Inspector
    public Transform parentTransform; // Asigna el padre en el que quieres que aparezca la imagen, por ejemplo un panel
    public Button spawnButton; // Asigna el botón para instanciar la imagen

    private GameObject instantiatedImage; // Referencia a la imagen instanciada

    void Start()
    {
        if (spawnButton != null) // Verifica que el botón de instanciar no sea null
        {
            spawnButton.onClick.AddListener(SpawnImage); // Añade el listener al botón de instanciar
        }
        else
        {
            Debug.LogError("El botón de instanciar no está asignado en el Inspector");
        }
    }

    void SpawnImage()
    {
        if (instantiatedImage == null && imagePrefab != null && parentTransform != null) // Verifica que no haya una imagen instanciada y que las referencias no sean null
        {
            instantiatedImage = Instantiate(imagePrefab, parentTransform); // Instancia el prefab en el parent asignado
            Button removeButton = instantiatedImage.GetComponentInChildren<Button>(); // Encuentra el botón de eliminar en el prefab instanciado

            if (removeButton != null) // Verifica que el botón de eliminar no sea null
            {
                removeButton.onClick.AddListener(RemoveImage); // Añade el listener al botón de eliminar
            }
            else
            {
                Debug.LogError("No se encontró el botón de eliminar en la imagen instanciada");
            }
        }
        else if (imagePrefab == null)
        {
            Debug.LogError("El prefab de la imagen no está asignado en el Inspector");
        }
        else if (parentTransform == null)
        {
            Debug.LogError("El parent transform no está asignado en el Inspector");
        }
    }

    void RemoveImage()
    {
        if (instantiatedImage != null) // Solo elimina si hay una imagen instanciada
        {
            Destroy(instantiatedImage); // Destruye la imagen instanciada
            instantiatedImage = null; // Resetea la referencia
        }
    }
}
