using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerAreaCardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public GameObject ZoomCardPrefab;

    private Transform Canvas;
    private bool isDragging;
    private bool isCardOverDropZone;
    private GameObject zoomCard;
    private CanvasGroup zoomCardCanvasGroup;

    private DropZoneManager dropZoneManager;

    private void Awake()
    {
        Canvas = FindObjectOfType<Canvas>().transform;
        dropZoneManager = FindObjectOfType<DropZoneManager>();

        if (ZoomCardPrefab == null)
        {
            Debug.LogError("Zoom card prefab not set in the inspector.", this);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter called.");
        if (!isDragging && !isCardOverDropZone && zoomCard == null)
        {
            Debug.Log("Creating zoom card.");
            CreateZoomCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit called.");
        if (!isDragging && !isCardOverDropZone)
        {
            Debug.Log("Destroying zoom card.");
            DestroyZoomCard();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag called.");
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
        zoomCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);  // Escalar ligeramente para efecto de zoom

        zoomCardCanvasGroup = zoomCard.GetComponent<CanvasGroup>();

        zoomCard.transform.SetAsLastSibling();

        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        RectTransform originalRectTransform = GetComponent<RectTransform>();

        if (rectTransform != null && originalRectTransform != null)
        {
            rectTransform.sizeDelta = originalRectTransform.sizeDelta;

            Vector3[] worldCorners = new Vector3[4];
            originalRectTransform.GetWorldCorners(worldCorners);

            Vector3 centerPosition = (worldCorners[0] + worldCorners[2]) / 2;
            centerPosition -= new Vector3(0, originalRectTransform.sizeDelta.y - 160, 0);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.position = centerPosition;

            Debug.Log($"ZoomCard created at position: {rectTransform.position}");
        }
        else
        {
            Debug.LogError("RectTransform is missing on the zoom card or the original card.");
        }

        if (zoomCardCanvasGroup != null)
        {
            zoomCardCanvasGroup.blocksRaycasts = false;
        }

        // Eliminar Rigidbody y Collider del clon
        Rigidbody2D zoomCardRigidbody = zoomCard.GetComponent<Rigidbody2D>();
        Debug.Log("HOLASAAAAAA", zoomCard);
        if (zoomCardRigidbody != null)
        {
            Destroy(zoomCardRigidbody);
            Debug.Log("Rigidbody removed from zoom card.");
        }

        BoxCollider2D zoomCardCollider = zoomCard.GetComponent<BoxCollider2D>();
        if (zoomCardCollider != null)
        {
            Destroy(zoomCardCollider);
            Debug.Log("Collider removed from zoom card.");
        }

        // Eliminar el script DragDrop del clon
        DragDrop dragDropScript = zoomCard.GetComponent<DragDrop>();
        if (dragDropScript != null)
        {
            Destroy(dragDropScript);
            Debug.Log("DragDrop script removed from zoom card.");
        }
    }

    private void DestroyZoomCard()
    {
        if (zoomCard != null)
        {
            zoomCard.transform.localScale = new Vector3(1, 1, 1);
            Destroy(zoomCard);
            zoomCard = null;
            Debug.Log("ZoomCard destroyed.");
        }
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        Debug.Log($"SetDragging called. Dragging: {dragging}");
        if (dragging)
        {
            DestroyZoomCard();
        }
    }

    public void SetCardOverDropZone(bool overDropZone)
    {
        isCardOverDropZone = overDropZone;
        Debug.Log($"SetCardOverDropZone called. OverDropZone: {overDropZone}");
        if (overDropZone)
        {
            DestroyZoomCard();
        }
    }
}
