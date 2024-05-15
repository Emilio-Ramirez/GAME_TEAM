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
        zoomCard = Instantiate(transform.gameObject, Canvas);
        zoomCard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);  // Escalar ligeramente para efecto de zoom
        zoomCardImage = zoomCard.GetComponent<Image>();
        zoomCardCanvasGroup = zoomCard.GetComponent<CanvasGroup>();
        zoomCardImage.sprite = GetComponent<Image>().sprite;
        zoomCard.transform.SetAsLastSibling(); 

        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        RectTransform originalRectTransform = GetComponent<RectTransform>();
    
        if (rectTransform != null && originalRectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(originalRectTransform.anchoredPosition.x, originalRectTransform.anchoredPosition.y + 500);
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
        zoomCard.transform.localScale = new Vector3(1, 1, 1);  // Restaurar el tamaño original
        if (zoomCard != null)
        {
            Destroy(zoomCard);
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
