using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging { get; private set; } = false;
    public bool IsOverDropZone { get; private set; } = false;
    public Transform StartParent { get; private set; }
    public Vector2 StartPosition { get; private set; }

    private Transform canvas;
    private GameObject dropZone;
    private PlayerAreaCardZoom cardZoom;
    private CanvasGroup canvasGroup;
    public GameObject DropZone;

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas").transform;
        cardZoom = GetComponent<PlayerAreaCardZoom>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartParent = transform.parent;
        StartPosition = transform.position;
        IsDragging = true;
        transform.SetParent(canvas, true);

        if (cardZoom != null)
        {
            cardZoom.SetDragging(true);
        }
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        canvasGroup.blocksRaycasts = true;

        if (IsOverDropZone && dropZone != null)
        {
            DropZoneManager dropZoneManager = dropZone.GetComponentInParent<DropZoneManager>();
            if (dropZoneManager != null)
            {
                int fromDropZoneIndex = System.Array.IndexOf(dropZoneManager.dropZones, StartParent.gameObject);
                int toDropZoneIndex = System.Array.IndexOf(dropZoneManager.dropZones, dropZone);
                
                // Verifica si la dropzone destino ya tiene una carta
                if (dropZoneManager.cardsInDropZone[toDropZoneIndex].Count > 0)
                {
                    Debug.LogWarning($"DropZone {toDropZoneIndex} already has a card. Cannot move the card.");
                    ReturnToStart();
                }
                else
                {
                    if (fromDropZoneIndex != -1 && toDropZoneIndex != -1 && fromDropZoneIndex != toDropZoneIndex)
                    {
                        dropZoneManager.MoveCardBetweenDropZones(gameObject, fromDropZoneIndex, toDropZoneIndex);
                    }
                    else
                    {
                        dropZoneManager.OnCardDropped(toDropZoneIndex, gameObject);
                    }

                    // Add a null check for dropZone before accessing its transform
                    if (dropZone != null)
                    {
                        transform.SetParent(dropZone.transform, true);
                        transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
        else
        {
            ReturnToStart();
        }

        if (cardZoom != null)
        {
            cardZoom.SetDragging(false);
        }
    }

    private void ReturnToStart()
    {
        transform.position = StartPosition;
        transform.SetParent(StartParent, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropZone"))
        {
            IsOverDropZone = true;
            dropZone = collision.gameObject;
            if (cardZoom != null)
            {
                cardZoom.SetCardOverDropZone(true);
            }
            Debug.Log($"Card entered DropZone: {dropZone.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropZone"))
        {
            IsOverDropZone = false;
            if (cardZoom != null)
            {
                cardZoom.SetCardOverDropZone(false);
            }
            dropZone = null;
            Debug.Log("Card exited DropZone");
        }
    }
}
