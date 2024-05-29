using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Linq;  // Añadir esta línea
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

    private List<Card> cards = new List<Card>();
    private List<Recipe> recipes = new List<Recipe>();
    private HashSet<int> validCardIdsForRecipeArea = new HashSet<int> { 1, 2, 3, 4 };
    private HashSet<int> validCardIdsForPlayerArea = new HashSet<int> { 1, 2, 3, 5 };
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
    }

    void Start()
    {
        Debug.Log("GameManager Start");
        Debug.Log("apiUrl: " + apiUrl);
        StartCoroutine(GetCards());
        StartCoroutine(GetRecipes());
        UpdateScoreUI();
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

        foreach (Card card in randomCards)
        {
            if (cardsAdded >= newCardsCount)
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
            IncreaseScore(recipe.valor_nutrimental);
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
