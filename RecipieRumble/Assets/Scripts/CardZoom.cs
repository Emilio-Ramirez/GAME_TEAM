using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ZoomCardPrefab;
    public Transform Canvas;
    public DropZoneManager dropZoneManager;

    private bool isHovering;
    private bool isCardOverDropZone;
    private bool isDragging;

    private GameObject zoomCard;
    private Image zoomCardImage;
    private CanvasGroup zoomCardCanvasGroup;

    private void Awake()
    {
        if (ZoomCardPrefab == null)
        {
            Debug.LogError("Zoom card prefab not set in the inspector.", this);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Verificar que no se esté arrastrando la tarjeta y que no esté sobre una zona de soltado
        if (!isDragging && !isCardOverDropZone)
        {
            isHovering = true;
            transform.localScale = new Vector3(2, 2, 2);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 100);

            zoomCard = Instantiate(ZoomCardPrefab, Canvas);
            zoomCardImage = zoomCard.GetComponent<Image>();
            zoomCardCanvasGroup = zoomCard.GetComponent<CanvasGroup>();
            zoomCardImage.sprite = GetComponent<Image>().sprite;
            zoomCard.transform.SetAsLastSibling();

            if (zoomCardCanvasGroup != null)
            {
                zoomCardCanvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Verificar que no se esté arrastrando la tarjeta y que no esté sobre una zona de soltado
        if (!isDragging && !isCardOverDropZone)
        {
            isHovering = false;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 100);
            if (zoomCard != null)
            {
                Destroy(zoomCard);
            }
        }
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (dragging)
        {
            isHovering = false;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 100);
            if (zoomCard != null)
            {
                Destroy(zoomCard);
            }
        }
    }

    public void SetCardOverDropZone(bool overDropZone)
    {
        isCardOverDropZone = overDropZone;
    }
}
