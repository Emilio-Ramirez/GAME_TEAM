// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using TMPro;

// public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
// {
//     public GameObject ZoomCardPrefab;
//     public GameObject recipeAreaImage;

//     private Transform Canvas;
//     private bool isDragging;
//     private bool isCardOverDropZone;
//     private GameObject zoomCard;
//     private CanvasGroup zoomCardCanvasGroup;

//     Agregar referencia al DropZoneManager
//     private DropZoneManager dropZoneManager;

//     private void Awake()
//     {
//         Buscar el Canvas en el momento de inicialización
//         Canvas = FindObjectOfType<Canvas>().transform;

//         Buscar el DropZoneManager
//         dropZoneManager = FindObjectOfType<DropZoneManager>();

//         if (ZoomCardPrefab == null)
//         {
//             Debug.LogError("Zoom card prefab not set in the inspector.", this);
//             return;
//         }
//     }

//     public void OnPointerEnter(PointerEventData eventData)
//     {
//         if (!isDragging && !isCardOverDropZone && zoomCard == null)
//         {
//             CreateZoomCard();
//         }
//     }

//     public void OnPointerExit(PointerEventData eventData)
//     {
//         Solo destruir la zoomCard si realmente no está en hover
//         if (!isDragging && !isCardOverDropZone)
//         {
//             DestroyZoomCard();
//         }
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         if (isDragging && zoomCard != null)
//         {
//             RectTransform rt = zoomCard.GetComponent<RectTransform>();
//             rt.anchoredPosition += eventData.delta;
//         }
//     }

//     private void CreateZoomCard()
//     {
//         if (ZoomCardPrefab == null)
//         {
//             Debug.LogError("ZoomCardPrefab is not assigned.");
//             return;
//         }

//         Verificar si la carta está en la recipe area
//         bool isInRecipeArea = false;
//         if (dropZoneManager != null && dropZoneManager.recipeArea != null)
//         {
//             foreach (Transform child in dropZoneManager.recipeArea)
//             {
//                 if (child == transform)
//                 {
//                     isInRecipeArea = true;
//                     break;
//                 }
//             }
//         }

//         Crear zoomCard como hijo de Canvas o recipeArea según corresponda
//         if (isInRecipeArea && recipeAreaImage != null)
//         {
//             zoomCard = Instantiate(recipeAreaImage, Canvas);
//             zoomCard.transform.localScale = new Vector3(2.0f, 1.5f, 1.5f);  // Hacer la imagen más larga en el eje Y
//             UpdateIngredientsText();  // Actualizar el texto de ingredientes
//         }
//         else
//         {
//             zoomCard = Instantiate(ZoomCardPrefab, Canvas);
//             zoomCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);  // Escalar ligeramente para efecto de zoom
//         }

//         zoomCardCanvasGroup = zoomCard.GetComponent<CanvasGroup>();

//         zoomCard.transform.SetAsLastSibling();

//         RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
//         RectTransform originalRectTransform = GetComponent<RectTransform>();

//         if (rectTransform != null && originalRectTransform != null)
//         {
//             rectTransform.sizeDelta = originalRectTransform.sizeDelta;

//             Vector3[] worldCorners = new Vector3[4];
//             originalRectTransform.GetWorldCorners(worldCorners);

//             Vector3 centerPosition = (worldCorners[0] + worldCorners[2]) / 2; // Promedio de la esquina inferior izquierda y la esquina superior derecha

//             if (isInRecipeArea)
//             {
//                 rectTransform.pivot = new Vector2(0.5f, 0.5f);  // Ajustar pivote si es necesario
//                 rectTransform.position = new Vector3(centerPosition.x, centerPosition.y - 20, centerPosition.z); // Ajustar un poco más abajo
//             }
//             else
//             {
//                 centerPosition -= new Vector3(0, originalRectTransform.sizeDelta.y - 160, 0); // Ajustar hacia arriba para otras áreas
//                 rectTransform.pivot = new Vector2(0.5f, 0.5f);  // Restaurar el pivote original
//                 rectTransform.position = centerPosition;
//             }

//             Añadir un Debug.Log para ver la posición del clon
//             Debug.Log($"ZoomCard created at position: {rectTransform.position}");
//         }
//         else
//         {
//             Debug.LogError("RectTransform is missing on the zoom card or the original card.");
//         }

//         if (zoomCardCanvasGroup != null)
//         {
//             zoomCardCanvasGroup.blocksRaycasts = false; // Permitir clicks a través del zoomCard
//         }
//     }

//     private void DestroyZoomCard()
//     {
//         if (zoomCard != null)
//         {
//             zoomCard.transform.localScale = new Vector3(1, 1, 1);  // Restaurar el tamaño original
//             Destroy(zoomCard);
//             zoomCard = null; // Asegurarse de que la referencia se elimine
//         }
//     }

//     public void SetDragging(bool dragging)
//     {
//         isDragging = dragging;
//         if (dragging)
//         {
//             DestroyZoomCard();
//         }
//     }

//     public void SetCardOverDropZone(bool overDropZone)
//     {
//         isCardOverDropZone = overDropZone;
//         if (overDropZone)
//         {
//             DestroyZoomCard();
//         }
//     }

//     private void UpdateIngredientsText()
//     {
//         if (zoomCard != null)
//         {
//             TextMeshProUGUI ingredientsText = zoomCard.GetComponentInChildren<TextMeshProUGUI>();

//             if (ingredientsText != null)
//             {
//                 int cardId;
//                 if (int.TryParse(gameObject.name, out cardId))
//                 {
//                     Recipe recipe = GameManager.Instance.recipes.FirstOrDefault(r => r.id_receta == cardId);
//                     if (recipe != null)
//                     {
//                         string ingredients = string.Join(", ", recipe.ingredientes.SelectMany(g => g.Value).ToList());
//                         ingredientsText.text = ingredients;
//                         Debug.Log($"Ingredients updated: {ingredients}");
//                     }
//                     else
//                     {
//                         Debug.LogWarning("Recipe not found for cardId: " + cardId);
//                     }
//                 }
//                 else
//                 {
//                     Debug.LogWarning("Unable to parse cardId from gameObject name: " + gameObject.name);
//                 }
//             }
//             else
//             {
//                 Debug.LogError("TextMeshProUGUI component not found in zoomCard.");
//             }
//         }
//     }
// }
