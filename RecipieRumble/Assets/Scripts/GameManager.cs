using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Linq;  // Añadir esta línea
using Newtonsoft.Json;
//MODIF

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

    private List<Card> cards = new List<Card>();
    private List<Recipe> recipes = new List<Recipe>();
    private HashSet<int> validCardIdsForRecipeArea = new HashSet<int> { 1, 2, 3, 4 };
    private HashSet<int> validCardIdsForPlayerArea = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 31, 45, 46, 47, 48 };
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
                List<Card> loadedCards = JsonHelper.FromJson<Card>(webRequest.downloadHandler.text).ToList();
                // Modificación para evitar el error de conversión
                cards = loadedCards.Select(card =>
                {
                    if (card.tipo == "Special")
                    {
                        return new SpecialCard(card.id_carta, card.nombre, card.valor_nutrimental, card.tipo, 1) as Card;
                    }
                    return card;
                }).ToList();

                Debug.Log(cards);

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
                recipes = JsonConvert.DeserializeObject<List<Recipe>>(webRequest.downloadHandler.text);
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

        int currentCardCount = playerCardParent.childCount;
        int newCardsCount = 4 - currentCardCount;

        if (newCardsCount <= 0)
        {
            Debug.Log("Player already has 4 cards. No new cards will be added.");
            return;
        }

        List<Card> randomCards = new List<Card>(cards);
        randomCards.Shuffle();

        int cardsAdded = 0;
        HashSet<int> existingCardIds = new HashSet<int>();
        HashSet<string> existingCardTypes = new HashSet<string>();

        // Recopilar los ids y tipos de las cartas actualmente en la mano del jugador
        foreach (Transform child in playerCardParent)
        {
            TMP_Text nameText = child.Find("NameText")?.GetComponent<TMP_Text>();
            TMP_Text typeText = child.Find("TypeText")?.GetComponent<TMP_Text>();
            if (nameText != null && typeText != null)
            {
                Card cardInHand = cards.FirstOrDefault(c => c.nombre == nameText.text);
                if (cardInHand != null)
                {
                    existingCardIds.Add(cardInHand.id_carta);
                    existingCardTypes.Add(typeText.text);
                }
            }
        }

        foreach (Card card in randomCards)
        {
            Debug.Log("CARDS CARDS CARDS: " + card.id_carta);

            if (cardsAdded >= newCardsCount)
            {
                break;
            }

            if (!validCardIdsForPlayerArea.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with invalid ID for player area: {card.id_carta}");
                continue;
            }

            if (existingCardIds.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with duplicate ID: {card.id_carta}");
                continue;
            }
            if (card.tipo != "Special")
            {
                if (existingCardTypes.Contains(card.tipo))
                {
                    Debug.LogWarning($"Skipping card with duplicate type: {card.tipo}");
                    continue;
                }
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
                string imagePath;
                if (card.id_carta == 47 || card.id_carta == 48)
                {
                    imagePath = $"Sprites/SpecialCards/{card.id_carta}";

                }
                else
                {
                    imagePath = $"Sprites/Picnic/{card.id_carta}";
                }
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

            // Añadir el id y tipo de la nueva carta a los ids y tipos existentes
            existingCardIds.Add(card.id_carta);
            existingCardTypes.Add(card.tipo);

            cardsAdded++;
        }

        UpdateScoreUI();
        dropZoneManager?.UpdateCardCount();
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
    public object id_libro;
}


