using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject cardPrefab;
    public GameObject recipeCardPrefab;
    public Transform playerCardParent;
    public Transform recipeCardParent;
    public string apiUrl = "http://localhost:3000/cartas"; 
    public string apiRecipeUrl = "http://localhost:3000/recetas";
    public DropZoneManager dropZoneManager;
    public TextMeshProUGUI scoreText;
    public Button deckButton;
    
    public GameObject fireworksPrefab; // Añadir una referencia al prefab de fuegos artificiales
    public Canvas canvas; // Añadir una referencia al Canvas

    private List<Card> cards = new List<Card>();
    public List<Recipe> recipes = new List<Recipe>();  // Hacer esta lista pública
    private HashSet<int> validCardIdsForRecipeArea = new HashSet<int>();
    private HashSet<int> validCardIdsForPlayerArea = new HashSet<int>();
    private int totalScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadValidCardIds();
    }

    void LoadValidCardIds()
    {
        Sprite[] picnicSprites = Resources.LoadAll<Sprite>("Sprites/Picnic");
        foreach (Sprite sprite in picnicSprites)
        {
            if (int.TryParse(sprite.name, out int id))
            {
                validCardIdsForPlayerArea.Add(id);
            }
        }

        Sprite[] eventDishesSprites = Resources.LoadAll<Sprite>("Sprites/Picnic/EventDishes");
        foreach (Sprite sprite in eventDishesSprites)
        {
            if (int.TryParse(sprite.name, out int id))
            {
                validCardIdsForRecipeArea.Add(id);
            }
        }
    }

    void Start()
    {
        Debug.Log("GameManager Start");
        Debug.Log("apiUrl: " + apiUrl);
        StartCoroutine(GetCards());
        StartCoroutine(GetRecipes());
        UpdateScoreUI();

        if (deckButton != null)
        {
            deckButton.onClick.AddListener(OnDeckButtonPressed);
        }
    }

    IEnumerator GetCards()
    {
        Debug.Log("Attempting to connect to URL: " + apiUrl);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}, URL: {apiUrl}");
            }
            else
            {
                Debug.Log("Connection successful. Response: " + webRequest.downloadHandler.text);
                cards = new List<Card>(JsonHelper.FromJson<Card>(webRequest.downloadHandler.text));
                AddCardsToRecipeArea();
                ShuffleAndAddNewCardsToPlayerArea();
            }
        }
    }

    IEnumerator GetRecipes()
    {
        Debug.Log("Attempting to connect to URL: " + apiRecipeUrl);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiRecipeUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}, URL: {apiRecipeUrl}");
            }
            else
            {
                Debug.Log("Connection successful. Response: " + webRequest.downloadHandler.text);
                List<Recipe> allRecipes = JsonConvert.DeserializeObject<List<Recipe>>(webRequest.downloadHandler.text);
                
                // Filtrar las recetas que pertenecen a belongs_to_level 1
                recipes = allRecipes.Where(r => r.belongs_to_level == 1).ToList();
                
                // Asignar id_receta como nombre de la receta y mostrar ingredientes
                foreach (var recipe in recipes)
                {
                    recipe.nombre = recipe.id_receta.ToString();
                    Debug.Log($"Receta ID: {recipe.id_receta}, Ingredientes: {string.Join(", ", recipe.ingredientes.SelectMany(ig => ig.Value))}");
                }
            }
        }
    }

    void AddCardsToRecipeArea()
    {
        Debug.Log("Adding cards to recipe area.");

        if (recipeCardParent == null)
        {
            Debug.LogError("recipeCardParent is null.");
            return;
        }

        foreach (Transform child in recipeCardParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            if (!validCardIdsForRecipeArea.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with invalid ID for recipe area: {card.id_carta}");
                continue;
            }

            GameObject newCard = Instantiate(recipeCardPrefab, recipeCardParent);
            if (newCard == null)
            {
                Debug.LogError("Failed to instantiate new card prefab for recipe area.");
                continue;
            }

            Image cardImage = newCard.GetComponent<Image>();
            if (cardImage != null)
            {
                string imagePath = $"Sprites/Picnic/EventDishes/{card.id_carta}";
                cardImage.sprite = Resources.Load<Sprite>(imagePath);
                if (cardImage.sprite == null)
                {
                    Debug.LogError($"Image not found for card: {imagePath}");
                }
                else
                {
                    Debug.Log($"Loaded image for card: {imagePath}");
                }
            }
            else
            {
                Debug.LogWarning("Failed to find Image component on the recipe card prefab.");
            }
        }
    }

    void ShuffleAndAddNewCardsToPlayerArea()
    {
        Debug.Log("Shuffling and adding new cards to player area.");

        if (playerCardParent == null)
        {
            Debug.LogError("playerCardParent is null.");
            return;
        }

        // Destruir las cartas actuales en la mano del jugador
        foreach (Transform child in playerCardParent)
        {
            Destroy(child.gameObject);
        }

        List<Card> randomCards = new List<Card>(cards);
        randomCards.Shuffle();

        int cardsAdded = 0;
        foreach (Card card in randomCards)
        {
            if (cardsAdded >= 4)
            {
                break;
            }

            if (!validCardIdsForPlayerArea.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with invalid ID for player area: {card.id_carta}");
                continue;
            }

            GameObject newCard = Instantiate(cardPrefab, playerCardParent);
            if (newCard == null)
            {
                Debug.LogError("Failed to instantiate new card prefab for player area.");
                continue;
            }

            TMP_Text nameText = newCard.transform.Find("NameText")?.GetComponent<TMP_Text>();
            TMP_Text valueText = newCard.transform.Find("ValueText")?.GetComponent<TMP_Text>();
            TMP_Text typeText = newCard.transform.Find("TypeText")?.GetComponent<TMP_Text>();
            Image cardImage = newCard.transform.Find("Image")?.GetComponent<Image>();

            if (nameText != null && valueText != null && typeText != null)
            {
                nameText.text = card.nombre;
                valueText.text = card.valor_nutrimental.ToString();
                typeText.text = card.tipo;
                newCard.name = card.nombre;
            }
            else
            {
                Debug.LogWarning("Failed to find one or more text components on the player card prefab.");
            }

            if (cardImage != null)
            {
                string imagePath = $"Sprites/Picnic/{card.id_carta}";
                cardImage.sprite = Resources.Load<Sprite>(imagePath);
                if (cardImage.sprite == null)
                {
                    Debug.LogError($"Image not found for card: {imagePath}");
                }
            }
            else
            {
                Debug.LogWarning("Failed to find Image component on the player card prefab.");
            }

            cardsAdded++;
        }

        UpdateScoreUI();
        dropZoneManager?.UpdateCardCount();
    }

    public void OnDeckButtonPressed()
    {
        deckButton.interactable = false;
        ShuffleAndAddNewCardsToPlayerArea();
        StartCoroutine(ReenableDeckButton());
    }

    IEnumerator ReenableDeckButton()
    {
        yield return new WaitForSeconds(1f);
        deckButton.interactable = true;
    }

    public void RefreshDeck()
    {
        StartCoroutine(GetCards());
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
    }

    public void CheckRecipeCombination()
    {
        List<string> ingredientsInRecipeArea = new List<string>();
        foreach (var dropZone in DropZoneManager.Instance.cardsInDropZone)
        {
            foreach (GameObject cardObject in dropZone)
            {
                string cardName = cardObject.name.ToLower();
                ingredientsInRecipeArea.Add(cardName);
                Debug.Log($"Added ingredient: {cardName}");
            }
        }

        Debug.Log($"Ingredients in recipe area: {string.Join(", ", ingredientsInRecipeArea)}");

        foreach (Recipe recipe in recipes)
        {
            Debug.Log($"Checking recipe ID: {recipe.id_receta}");
            bool isMatch = true;
            List<string> remainingIngredients = new List<string>(ingredientsInRecipeArea);
            int totalNutritionalValue = 0;

            foreach (var ingredientGroup in recipe.ingredientes)
            {
                Debug.Log($"Checking ingredient group: {ingredientGroup.Key}");
                bool groupMatched = false;

                foreach (string ingredient in ingredientGroup.Value)
                {
                    string lowerCaseIngredient = ingredient.ToLower();
                    if (remainingIngredients.Contains(lowerCaseIngredient))
                    {
                        Debug.Log($"Found ingredient: {lowerCaseIngredient}");

                        var card = cards.FirstOrDefault(c => c.nombre.ToLower() == lowerCaseIngredient);
                        if (card != null)
                        {
                            totalNutritionalValue += card.valor_nutrimental;
                        }

                        remainingIngredients.Remove(lowerCaseIngredient);
                        groupMatched = true;
                        break;
                    }
                }

                if (!groupMatched)
                {
                    Debug.Log($"No matching ingredient found for group: {ingredientGroup.Key}");
                    isMatch = false;
                    break;
                }
            }

            if (isMatch && remainingIngredients.Count == 0)
            {
                Debug.Log($"Combination found for recipe ID: {recipe.id_receta}");
                IncreaseScore(totalNutritionalValue);
                return;
            }
        }

        Debug.Log("No valid recipe combination found.");
    }

    private void IncreaseScore(int nutritionalValue)
    {
        totalScore += nutritionalValue;
        UpdateScoreUI();
        Debug.Log($"Score increased by {nutritionalValue}. New score: {totalScore}");
        
        foreach (var cardList in DropZoneManager.Instance.cardsInDropZone)
        {
            foreach (GameObject card in cardList)
            {
                Destroy(card);
            }
            cardList.Clear();
        }

        for (int i = 0; i < DropZoneManager.Instance.numberOfDropZones; i++)
        {
            DropZoneManager.Instance.turnsLeft[i] = DropZoneManager.Instance.initialTurns[i];
        }

        // Instanciar fuegos artificiales como hijo del Canvas y centrarlos
        if (fireworksPrefab != null && canvas != null)
        {
            GameObject fireworks = Instantiate(fireworksPrefab, canvas.transform);
            RectTransform rectTransform = fireworks.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero; // Centrar en el Canvas
            rectTransform.localScale = Vector3.one; // Asegurar que el tamaño sea correcto
            rectTransform.SetAsLastSibling(); // Asegurar que los fuegos artificiales estén al frente
        }

        ShuffleAndAddNewCardsToPlayerArea();
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

[System.Serializable]
public class Recipe
{
    public int id_receta;
    public bool es_principal;
    public string nombre;
    public int valor_nutrimental;
    public Dictionary<string, List<string>> ingredientes;
    public int belongs_to_level;
    public object id_libro;
}
