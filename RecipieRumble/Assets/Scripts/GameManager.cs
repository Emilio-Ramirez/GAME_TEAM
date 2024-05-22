using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab de la carta
    public Transform cardParent; // Transform del padre donde se añadirán las cartas
    public string apiUrl = "http://localhost:3000/cartas"; 

    private List<Card> cards = new List<Card>();

    void Start()
    {
        StartCoroutine(GetCards());
    }

    IEnumerator GetCards()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}, URL: {apiUrl}");
                
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text); // Para verificar la respuesta
                cards = new List<Card>(JsonHelper.FromJson<Card>(webRequest.downloadHandler.text));
                PopulateDeck();
            }
        }
    }

    void PopulateDeck()
    {
        foreach (Card card in cards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);

            // Encontrar y asignar los textos dentro del prefab
            Text nameText = newCard.transform.Find("NameText").GetComponent<Text>();
            Text valueText = newCard.transform.Find("ValueText").GetComponent<Text>();
            Text typeText = newCard.transform.Find("TypeText").GetComponent<Text>();

            nameText.text = card.nombre;
            valueText.text = card.valor_nutrimental.ToString();
            typeText.text = card.tipo;
        }
    }
}

// Helper class to parse JSON array
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
