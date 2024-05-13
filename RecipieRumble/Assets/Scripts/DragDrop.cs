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
        // Verificar si canvas es null
        if (canvas == null)
        {
            Debug.LogError("Canvas is null. Please assign it in Awake or ensure it's correctly found.");
            return; // Salir del método para evitar más errores
        }

        // Verificar si cardZoom es null
        if (cardZoom == null)
        {
            Debug.LogError("CardZoom component is missing or not attached to this object.");
            return; // Salir del método para evitar más errores
        }

        // Verificar si canvasGroup es null
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is null. A CanvasGroup component is required.");
            return; // Salir del método para evitar más errores
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
                // Solo establecer el padre al dropZone si la zona está desocupada
                transform.SetParent(dropZone.transform, true); // Cambio de 'false' a 'true' para mantener la posición global y ajustar la local automáticamente.
                transform.localPosition = Vector3.zero;  // Asegúrate de que la posición local sea cero para alinear correctamente la carta con su nuevo padre.
                dropZoneManager.OnCardDropped(dropZoneIndex, gameObject);
            }
            else
            {
                // Devolver la carta a su área de jugador si la drop zone está ocupada o si no hay dropZone válida.
                ReturnToStart();
            }
        }
    }
    else
    {
        // Devolver la carta a su área de jugador si no se soltó en una drop zone válida.
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

    void Update()
{
    if (Input.GetMouseButtonDown(1) && IsOverDropZone)  // Verificar si el botón derecho del ratón es presionado
    {
        ReturnCardToPlayerArea();
    }
}

private void ReturnCardToPlayerArea()
{
    if (transform.parent == dropZone.transform)  // Asegurarse de que la carta está actualmente en la drop zone
    {
        transform.SetParent(StartParent);  // Mover la carta de regreso al padre inicial, que debe ser la 'player area'
        transform.position = StartPosition;  // Restablecer la posición a la inicial (opcional, dependiendo del diseño de la UI)
        GetComponent<RectTransform>().SetAsLastSibling();  // Opcional: asegura que la carta se mueva al final del GridLayout
        canvasGroup.blocksRaycasts = true;  // Re-habilitar el raycast para permitir más interacciones
    }
}

    
}
