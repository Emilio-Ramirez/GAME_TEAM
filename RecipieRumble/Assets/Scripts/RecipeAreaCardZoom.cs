using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class RecipeAreaCardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject recipeAreaImagePrefab;
    public string apiRecipeUrl = "http://localhost:3000/recetas"; // URL de tu API

    private int recipeId; // Declarar recipeId como variable de instancia
    private Transform Canvas;
    private GameObject zoomCard;
    private bool isPointerInside = false;
    private const int LEVEL = 1; // Nivel fijo

    private static Dictionary<int, RecipeData> level1Recipes = new Dictionary<int, RecipeData>(); // Almacena recetas del nivel 1
    private static HashSet<int> assignedRecipeIds = new HashSet<int> { 1, 2, 3, 4 }; // Almacena los IDs de recetas asignadas

    // Define colors for each ingredient type
    private readonly Color proteinColor = new Color32(200, 88, 77, 255); // Red
    private readonly Color sideColor = new Color32(178, 133, 78, 255); // Yellow
    private readonly Color vegetableColor = new Color32(125, 143, 92, 255); // Green
    private readonly Color utensilsColor = new Color32(137, 178, 218, 255); // Blue

    private void Start()
    {
        Canvas = FindObjectOfType<Canvas>().transform;

        if (recipeAreaImagePrefab == null)
        {
            Debug.LogError("Recipe area image prefab not set in the inspector.", this);
            return;
        }

        // Cargar recetas del nivel 1 si no están ya cargadas
        if (level1Recipes.Count == 0)
        {
            StartCoroutine(LoadLevel1Recipes());
        }
        else
        {
            // Asignar el recipeId basado en la posición de la carta en el padre
            AssignRecipeId();
        }
    }

    private IEnumerator LoadLevel1Recipes()
    {
        string url = $"{apiRecipeUrl}?belongs_to_level={LEVEL}";

        Debug.Log($"Fetching level {LEVEL} recipes from URL: {url}");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}, URL: {url}");
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log($"Response for level {LEVEL} recipes: {jsonResponse}");

                List<RecipeData> recipes = JsonConvert.DeserializeObject<List<RecipeData>>(jsonResponse);

                foreach (var recipe in recipes)
                {
                    if (recipe.belongs_to_level == LEVEL)
                    {
                        level1Recipes[recipe.id_receta] = recipe;
                    }
                }

                Debug.Log($"Loaded {level1Recipes.Count} recipes for level {LEVEL}");

                // Asignar el recipeId basado en la posición de la carta en el padre
                AssignRecipeId();
            }
        }
    }

    private void AssignRecipeId()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogError("Parent transform is not assigned.");
            return;
        }

        int siblingIndex = transform.GetSiblingIndex();
        recipeId = siblingIndex % 4 + 1; // Asignar el id_receta de manera cíclica entre 1 y 4

        // Verificar si el recipeId ya ha sido asignado antes
        if (!assignedRecipeIds.Contains(recipeId))
        {
            Debug.Log($"RecipeId {recipeId} ya fue asignado previamente, no se reasignará.");
            return;
        }

        Debug.Log($"Asignado recipeId {recipeId} a la carta en la posición {siblingIndex}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        if (zoomCard == null && recipeId <= 4) // Solo crear la carta de zoom si el recipeId es válido
        {
            CreateZoomCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        StartCoroutine(CheckPointerExit());
    }

    private IEnumerator CheckPointerExit()
    {
        yield return new WaitForSeconds(0.1f);

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        bool isStillOverCard = raycastResults.Any(r => r.gameObject == gameObject);

        if (!isStillOverCard)
        {
            if (zoomCard != null)
            {
                DestroyZoomCard();
            }
        }
        else
        {
            isPointerInside = true;
        }
    }

    private void CreateZoomCard()
    {
        if (recipeAreaImagePrefab == null)
        {
            Debug.LogError("Recipe area image prefab is not assigned.");
            return;
        }

        zoomCard = Instantiate(recipeAreaImagePrefab, Canvas);
        zoomCard.transform.localScale = new Vector3(2.0f, 1.5f, 1.5f);
        zoomCard.transform.SetAsLastSibling();

        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        RectTransform originalRectTransform = GetComponent<RectTransform>();

        if (rectTransform != null && originalRectTransform != null)
        {
            rectTransform.sizeDelta = originalRectTransform.sizeDelta;

            Vector3[] worldCorners = new Vector3[4];
            originalRectTransform.GetWorldCorners(worldCorners);

            Vector3 centerPosition = (worldCorners[0] + worldCorners[2]) / 2;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.position = new Vector3(centerPosition.x, centerPosition.y - 20, centerPosition.z);
        }
        else
        {
            Debug.LogError("RectTransform is missing on the zoom card or the original card.");
        }

        // Temporarily disable raycasts to avoid additional events
        CanvasGroup canvasGroup = zoomCard.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        // Re-enable raycasts after positioning
        //canvasGroup.blocksRaycasts = true;

        // Display ingredients for the specific recipe from the pre-loaded data
        DisplayIngredients(recipeId);
    }

    private void DestroyZoomCard()
    {
        if (zoomCard != null)
        {
            Destroy(zoomCard);
            zoomCard = null;
        }
    }

    private void DisplayIngredients(int recipeId)
    {
        if (level1Recipes.TryGetValue(recipeId, out RecipeData recipe))
        {
            // Formatear los ingredientes
            string ingredientsText = $"<b>Ingredientes:</b>\n";
            if (recipe.ingredientes.ContainsKey("protein"))
            {
                ingredientsText += $"<color=#{ColorUtility.ToHtmlStringRGBA(proteinColor)}>Protein: {string.Join(", ", recipe.ingredientes["protein"])}</color>\n";
            }
            if (recipe.ingredientes.ContainsKey("side"))
            {
                ingredientsText += $"<color=#{ColorUtility.ToHtmlStringRGBA(sideColor)}>Side: {string.Join(", ", recipe.ingredientes["side"])}</color>\n";
            }
            if (recipe.ingredientes.ContainsKey("verduras"))
            {
                ingredientsText += $"<color=#{ColorUtility.ToHtmlStringRGBA(vegetableColor)}>Vegetable: {string.Join(", ", recipe.ingredientes["verduras"])}</color>\n";
            }
            if (recipe.ingredientes.ContainsKey("utils"))
            {
                ingredientsText += $"<color=#{ColorUtility.ToHtmlStringRGBA(utensilsColor)}>Utensils: {string.Join(", ", recipe.ingredientes["utils"])}</color>\n";
            }

            Debug.Log($"Fetched ingredients for recipeId {recipeId}: {ingredientsText}");

            TextMeshProUGUI ingredientsTextComponent = zoomCard.GetComponentInChildren<TextMeshProUGUI>();
            if (ingredientsTextComponent != null)
            {
                ingredientsTextComponent.text = ingredientsText;
            }
            else
            {
                Debug.LogError("No TextMeshProUGUI component found in zoomCard.");
            }
        }
        else
        {
            Debug.LogError($"No recipes found for recipeId {recipeId} or the recipe does not belong to level {LEVEL}.");
        }
    }
}

[System.Serializable]
public class RecipeData
{
    public int id_receta;
    public bool es_principal;
    public int belongs_to_level;
    public Dictionary<string, List<string>> ingredientes;
}
