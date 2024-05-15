using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public GameObject ZoomCardPrefab;
    private Transform Canvas;

    private bool isDragging;
    private bool isCardOverDropZone;
    private GameObject zoomCard;
    private Image zoomCardImage;
    private CanvasGroup zoomCardCanvasGroup;

    private void Awake()
    {
        // Buscar el Canvas en el momento de inicialización
        Canvas = FindObjectOfType<Canvas>().transform;

        if (ZoomCardPrefab == null)
        {
            Debug.LogError("Zoom card prefab not set in the inspector.", this);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging && !isCardOverDropZone)
        {
            CreateZoomCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyZoomCard();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && zoomCard != null)
        {
            RectTransform rt = zoomCard.GetComponent<RectTransform>();
            rt.anchoredPosition += eventData.delta;
        }
    }

    private void CreateZoomCard()
    {
        if (ZoomCardPrefab == null)
        {
            Debug.LogError("ZoomCardPrefab is not assigned.");
            return;
        }

        zoomCard = Instantiate(ZoomCardPrefab, Canvas);
        zoomCard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);  // Escalar ligeramente para efecto de zoom

        zoomCardImage = zoomCard.GetComponent<Image>();
        zoomCardCanvasGroup = zoomCard.GetComponent<CanvasGroup>();

        Image originalImage = GetComponent<Image>();
        if (originalImage == null)
        {
            Debug.LogError("Original card is missing Image component.");
            return;
        }

        zoomCardImage.sprite = originalImage.sprite;
        zoomCard.transform.SetAsLastSibling();

        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        RectTransform originalRectTransform = GetComponent<RectTransform>();

        if (rectTransform != null && originalRectTransform != null)
        {
            rectTransform.sizeDelta = originalRectTransform.sizeDelta;

            Vector3[] worldCorners = new Vector3[4];
            originalRectTransform.GetWorldCorners(worldCorners);

            Vector3 centerPosition = (worldCorners[0] + worldCorners[2]) / 2; // Promedio de la esquina inferior izquierda y la esquina superior derecha

            // Identificar si la carta está en la "player zone" o en la "recipe zone"
            if (transform.parent.name == "PlayerZone")
            {
                // Desplazar hacia arriba en la "player zone"
                centerPosition += new Vector3(0, originalRectTransform.sizeDelta.y, 0); // Ajustar este valor según sea necesario
            }
            else if (transform.parent.name == "RecipeZone")
            {
                // Mantener el comportamiento actual en la "recipe zone"
                centerPosition -= new Vector3(0, originalRectTransform.sizeDelta.y / 2, 0); // Ajustar este valor según sea necesario
            }

            rectTransform.position = centerPosition;

            // Añadir un Debug.Log para ver la posición del clon
            Debug.Log($"ZoomCard created at position: {rectTransform.position} in parent: {transform.parent.name}");
        }
        else
        {
            Debug.LogError("RectTransform is missing on the zoom card or the original card.");
        }

        if (zoomCardCanvasGroup != null)
        {
            zoomCardCanvasGroup.blocksRaycasts = false; // Permitir clicks a través del zoomCard
        }
    }

    private void DestroyZoomCard()
    {
        if (zoomCard != null)
        {
            zoomCard.transform.localScale = new Vector3(1, 1, 1);  // Restaurar el tamaño original
            Destroy(zoomCard);
            zoomCard = null; // Asegurarse de que la referencia se elimine
        }
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (dragging)
        {
            DestroyZoomCard();
        }
    }

    public void SetCardOverDropZone(bool overDropZone)
    {
        isCardOverDropZone = overDropZone;
        if (overDropZone)
        {
            DestroyZoomCard();
        }
    }
}
