using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging { get; private set; } = false;
    public bool IsOverDropZone { get; private set; } = false;
    public Transform StartParent { get; private set; }
    public Vector2 StartPosition { get; private set; }

    private Transform canvas;
    private GameObject dropZone;
    private CardZoom cardZoom;
    private CanvasGroup canvasGroup;
    public GameObject DropZone;

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas").transform;
        cardZoom = GetComponent<CardZoom>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        dropZone = GameObject.Find("DropZone");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is null. Please assign it in Awake or ensure it's correctly found.");
            return;
        }

        if (cardZoom == null)
        {
            Debug.LogError("CardZoom component is missing or not attached to this object.");
            return;
        }

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is null. A CanvasGroup component is required.");
            return;
        }

        StartParent = transform.parent;
        StartPosition = transform.position;
        IsDragging = true;
        transform.SetParent(canvas, true);

        cardZoom.SetDragging(true);
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
                int dropZoneIndex = System.Array.IndexOf(dropZoneManager.dropZones, dropZone);
                if (dropZoneIndex != -1 && dropZoneManager.cardsInDropZone[dropZoneIndex].Count == 0)
                {
                    transform.SetParent(dropZone.transform, true);
                    transform.localPosition = Vector3.zero;
                    dropZoneManager.OnCardDropped(dropZoneIndex, gameObject);
                }
                else
                {
                    ReturnToStart();
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
