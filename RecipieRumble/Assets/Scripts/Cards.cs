using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardsData
{
    public List<Card> cards;
}

public class Cards : MonoBehaviour
{
    public CardsData cards;

    public List<GameObject> cardImages; // Asigna las imágenes de las cartas en el inspector

    private string jsonData = @"
    {
        ""cards"": [
            {
                ""id"": 1,
                ""nombre"": ""Manzana"",
                ""valor_nutrimental"": ""Vitamina C, Fibra"",
                ""tipo"": 1
            },
            {
                ""id"": 2,
                ""nombre"": ""Plátano"",
                ""valor_nutrimental"": ""Potasio, Vitamina B6"",
                ""tipo"": 1
            },
            {
                ""id"": 3,
                ""nombre"": ""Brócoli"",
                ""valor_nutrimental"": ""Vitamina K, Vitamina C, Folato"",
                ""tipo"": 2
            },
            {
                ""id"": 4,
                ""nombre"": ""Zanahoria"",
                ""valor_nutrimental"": ""Vitamina A, Betacaroteno"",
                ""tipo"": 2
            },
            {
                ""id"": 5,
                ""nombre"": ""Salmón"",
                ""valor_nutrimental"": ""Proteína, Omega-3"",
                ""tipo"": 3
            }
        ]
    }
    ";

    void Start()
    {
        LoadCardsFromJson();
        AssignCardsToImages();
    }

    void LoadCardsFromJson()
    {
        // Deserializar el JSON a un objeto CardsData
        cards = JsonUtility.FromJson<CardsData>(jsonData);
    }

    void AssignCardsToImages()
    {
        if (cards != null && cards.cards.Count > 0)
        {
            for (int i = 0; i < cardImages.Count && i < cards.cards.Count; i++)
            {
                Card card = cards.cards[i];
                GameObject cardImage = cardImages[i];

                // Aquí puedes asignar el texto de depuración para mostrar la información de la carta
                Debug.Log($"ID: {card.id}, Nombre: {card.nombre}, Valor Nutrimental: {card.valor_nutrimental}, Tipo: {card.tipo}");

                // Puedes asignar más propiedades o realizar acciones específicas con cardImage
                // Por ejemplo, cambiar el nombre del objeto de la imagen
                cardImage.name = card.nombre;
                // Si tienes un componente de texto, puedes actualizarlo con la información de la carta
                // cardImage.GetComponentInChildren<Text>().text = card.nombre;
            }
        }
    }
}
