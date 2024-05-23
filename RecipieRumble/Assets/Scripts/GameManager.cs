using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent;
    public string apiUrl = "http://localhost:3000/cartas";

    private List<Card> cards = new List<Card>();
    private HashSet<int> validCardIds = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 31, 45, 46 };

    void Start()
    {
        Debug.Log("GameManager Start");
        Debug.Log("apiUrl: " + apiUrl);
        StartCoroutine(GetCards());
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
                PopulateDeck();
            }
        }
    }

    void PopulateDeck()
    {
        Debug.Log("Populating Deck with " + cards.Count + " cards.");

        // Limpiar cartas antiguas antes de añadir nuevas
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        // Mostrar solo las primeras 4 cartas válidas
        int cardsAdded = 0;
        List<Card> randomCards = new List<Card>(cards);
        randomCards.Shuffle(); // Método de extensión para mezclar la lista de cartas

        foreach (Card card in randomCards)
        {
            if (cardsAdded >= 4)
            {
                break;
            }

            if (!validCardIds.Contains(card.id_carta))
            {
                Debug.LogWarning($"Skipping card with invalid ID: {card.id_carta}");
                continue;
            }

            GameObject newCard = Instantiate(cardPrefab, cardParent);
            Debug.Log($"Instantiated new card: {newCard.name}");

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

            if (cardImage != null)
            {
                string imagePath = $"Sprites/Picnic/{card.id_carta}";
                cardImage.sprite = Resources.Load<Sprite>(imagePath);
                if (cardImage.sprite == null)
                    Debug.LogError($"Image not found for card: {imagePath}");
            }

            cardsAdded++;
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
