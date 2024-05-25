using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform playerCardParent;
    public string apiUrl = "http://localhost:3000/cartas";
    public DropZoneManager dropZoneManager;
    public TextMeshProUGUI scoreText;

    private List<Card> cards = new List<Card>();
    private HashSet<int> validCardIds = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 31, 45, 46 };
    private int totalScore = 0;

    void Start()
    {
        Debug.Log("GameManager Start");
        Debug.Log("apiUrl: " + apiUrl);
        StartCoroutine(GetCards());
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
                ShuffleAndAddNewCards();
            }
        }
    }

    void ShuffleAndAddNewCards()
    {
        Debug.Log("Shuffling and adding new cards.");

        int currentCardCount = playerCardParent.childCount;
        int newCardsCount = 4 - currentCardCount;

        if (newCardsCount <= 0)
        {
            Debug.Log("Player already has 4 cards. No new cards will be added.");
            return;
        }

        List<Card> randomCards = new List<Card>(cards);
        randomCards.Shuffle(); // Mezclar la lista de cartas

        int cardsAdded = 0;

        foreach (Card card in randomCards)
        {
            if (cardsAdded >= newCardsCount)
            {
                break;
            }

            if (!validCardIds.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with invalid ID: {card.id_carta}");
                continue;
            }

            GameObject newCard = Instantiate(cardPrefab, playerCardParent); // Añadir solo nuevas cartas
            if (newCard == null)
            {
                Debug.LogError("Failed to instantiate new card prefab.");
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
            }
            else
            {
                Debug.LogWarning("Failed to find one or more text components on the card prefab.");
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
                Debug.LogWarning("Failed to find Image component on the card prefab.");
            }

            // Añadir valor nutricional al puntaje total
            totalScore += card.valor_nutrimental;
            Debug.Log($"Added {card.valor_nutrimental} points. Total Score: {totalScore}");
            UpdateScoreUI(); // Actualizar la UI con el nuevo puntaje

            cardsAdded++;
        }

        // Actualizar el conteo de cartas en el DropZoneManager
        dropZoneManager.UpdateCardCount();
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
