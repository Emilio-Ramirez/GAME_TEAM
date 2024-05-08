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

    void Start(){
        dropZone = GameObject.Find("DropZone");
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
        // Ensure card is in front of other UI elements
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        // Reset the canvas group settings
        canvasGroup.blocksRaycasts = true;
        
        if (IsOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else
        {
            transform.position = StartPosition;
            transform.SetParent(StartParent, false);
        }
        if (cardZoom != null)
        {
            cardZoom.SetDragging(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsOverDropZone = false;
        dropZone = null;
    }
}
