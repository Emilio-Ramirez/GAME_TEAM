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
    private Timer timer;

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

        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("No Timer component found in the scene.");
        }

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
            GameObject nutritionalValueText = newCard.transform.Find("ValueText")?.gameObject; // El objeto que contiene el valor nutricional
            GameObject nutritionalValueTitle = newCard.transform.Find("Text")?.gameObject; // El objeto "Text" que contiene "Nutritional Value:"

            if (nameText != null && valueText != null && typeText != null)
            {
                nameText.text = card.nombre;
                if (card.nombre != "points" && card.nombre != "time")
                {
                    valueText.text = card.valor_nutrimental.ToString();
                    if (nutritionalValueTitle != null)
                    {
                        nutritionalValueTitle.SetActive(true);
                    }
                    if (nutritionalValueText != null)
                    {
                        nutritionalValueText.SetActive(true);
                    }
                }
                else
                {
                    if (nutritionalValueTitle != null)
                    {
                        nutritionalValueTitle.SetActive(false);
                    }
                    if (nutritionalValueText != null)
                    {
                        nutritionalValueText.SetActive(false);
                    }
                }
                typeText.text = card.tipo;
                newCard.name = card.nombre;
            }
            else
            {
                Debug.LogWarning("Failed to find one or more text components on the player card prefab.");
            }

            if (cardImage != null)
            {
                string imagePath;
                string imagePath_special;
                imagePath = $"Sprites/Picnic/{card.id_carta}";

                cardImage.sprite = Resources.Load<Sprite>(imagePath);
                if (cardImage.sprite == null)
                {
                    Debug.LogError($"Image not found for card: {imagePath}");
                }
                else
                {
                    Debug.Log($"Image FOUND FOUND FOUND for: {imagePath}");
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
                if (cardName == "points" || cardName == "time")
                {
                    Debug.Log($" OBJOBJOBJOBJOBJOBJOBJOBJO: {cardObject}");
                    SpecialAbilityUse(cardObject, cardName);
                    Debug.Log($"Used Special HabilityYYYYYYYYYY");
                    Destroy(cardObject);
                    Debug.Log("Card DESTROYEDDDDDDDD");
                }
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

                        // Buscar la carta correspondiente para obtener el valor nutrimental
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
        StartCoroutine(ScoreFlash());

        Debug.Log($"Score increased by {nutritionalValue}. New score: {totalScore}");

        // Descartar todas las cartas del dropzone
        foreach (var cardList in DropZoneManager.Instance.cardsInDropZone)
        {
            foreach (GameObject card in cardList)
            {
                Destroy(card);
            }
            cardList.Clear();
        }

        // Resetear los turnos
        for (int i = 0; i < DropZoneManager.Instance.numberOfDropZones; i++)
        {
            DropZoneManager.Instance.turnsLeft[i] = DropZoneManager.Instance.initialTurns[i];
        }

        // Popular las cartas de la mano
        ShuffleAndAddNewCardsToPlayerArea();
    }

    private IEnumerator ScoreFlash()
    {
        Color originalColor = scoreText.color;
        Color flashColor = Color.yellow;
        float flashDuration = 3.0f;
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 flashScale = originalScale * 1.2f;

        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            scoreText.color = flashColor;
            scoreText.transform.localScale = flashScale;
            yield return new WaitForSeconds(0.2f);
            scoreText.color = originalColor;
            scoreText.transform.localScale = originalScale;
            yield return new WaitForSeconds(0.2f);
            elapsedTime += 0.4f;
        }

        scoreText.color = originalColor;
        scoreText.transform.localScale = originalScale;
    }

    private void SpecialAbilityUse(GameObject card, string cardName)
    {
        if (cardName == "points")
        {
            totalScore = totalScore - 10;
            UpdateScoreUI();
            Debug.Log("Decreased Score");
        }
        if (cardName == "time")
        {
            IncreaseGameTimer();
            Debug.Log("Timmer increased");
        }
    }

    public void IncreaseGameTimer()
    {
        if (timer != null)
        {
            timer.IncreaseTimer();
        }
        else
        {
            Debug.LogWarning("Timer reference is not set in GameManager.");
        }
    }

    public void UpdateUserScores()
    {
        StartCoroutine(UpdateUserScoresCoroutine());
    }

    private IEnumerator UpdateUserScoresCoroutine()
    {
        // Obtener el puntaje total y el nivel actual del usuario
        int puntajeMaximo = totalScore;
        float averageDishesPerEvent = 1;  // Implementa esta funci�n seg�n tu l�gica
        
        // Obtener el nivel actual desde PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);  // Valor predeterminado de 1 si no se encuentra
        string nivel = GetLevelNameFromIndex(currentLevel);  // Convertir el �ndice de nivel a su nombre correspondiente


        // Crear una instancia de ScoresData con los datos a enviar
        ScoresData scores = new ScoresData
        {
            puntaje_maximo = puntajeMaximo,
            average_dishes_per_event = averageDishesPerEvent,
            nivel = string.IsNullOrEmpty(nivel) ? "unknown" : nivel
        };


        // Crear la solicitud POST
        string url = "http://localhost:3000/update-scores";
        var request = new UnityWebRequest(url, "POST");
        string jsonData = JsonConvert.SerializeObject(scores);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Agregar el token de sesi�n a los encabezados de la solicitud
        string token = PlayerPrefs.GetString("token");
        request.SetRequestHeader("token", token.Trim('"'));

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User scores updated successfully");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error updating user scores: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }

    // Método para convertir el índice de nivel a su nombre correspondiente
    private string GetLevelNameFromIndex(int levelIndex)
    {
        switch (levelIndex)
        {
            case 1:
                return "wedding";
            case 2:
                return "picnic";
            case 3:
                return "christmas_dinner";
            default:
                return "unknown"; // Valor predeterminado si el índice no coincide con ningún nivel válido
        }
    }
}



// Extensiones para la utilidad de JSON y la función de mezclar listas (shuffle)
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

// Clase Recipe para representar las recetas
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

[System.Serializable]
public class ScoresData
{
    public int puntaje_maximo;
    public float average_dishes_per_event;
    public string nivel;
}
