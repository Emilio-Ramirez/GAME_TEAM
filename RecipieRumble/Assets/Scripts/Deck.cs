using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public Transform RecipeArea; 
    public GameObject PlayerArea; 
    public DropZoneManager dropZoneManager;
    public Button deckButton; // Referencia al botón de la baraja

    List<GameObject> playerCards = new List<GameObject>(); // Lista para las cartas del jugador
    List<GameObject> recipeCardPrefabs = new List<GameObject>(); // Lista para los prefabs de cartas de receta
    List<GameObject> instantiatedRecipeCards = new List<GameObject>(); // Lista para las cartas de receta instanciadas

    void Start()
    {
        if (!Card1 || !Card2 || !RecipeArea || !PlayerArea || !dropZoneManager || !deckButton)
        {
            Debug.LogError("Uno o más componentes esenciales no están asignados en el script Deck.");
            return; // Detener la ejecución si faltan componentes críticos
        }

        // Inicializar las listas de cartas del jugador
        playerCards.Add(Card2); // Solo agregar Card2 para el jugador
        
        // Inicializar las listas de cartas de receta
        recipeCardPrefabs.Add(Card1); // Solo agregar Card1 para la receta

        InitializeRecipeCards(); // Configurar las cartas de receta solo una vez
    }

    void InitializeRecipeCards()
    {
        for (int i = 0; i < 4; i++) // Exactamente 4 cartas de receta
        {
            GameObject recipeCard = Instantiate(recipeCardPrefabs[Random.Range(0, recipeCardPrefabs.Count)], Vector3.zero, Quaternion.identity);
            recipeCard.transform.SetParent(RecipeArea, false);
            recipeCard.name = "RecipeCard_" + i;
            instantiatedRecipeCards.Add(recipeCard); // Mantener un seguimiento de las cartas de receta instanciadas
        }
    }

    public void OnClick()
    {
        int cardsInHand = PlayerArea.transform.childCount;
        int cardsNeeded = 4 - cardsInHand; // Calcular cuántas cartas se necesitan para llegar a 4

        for (int i = 0; i < cardsNeeded; i++)
        {
            GameObject playerCard = Instantiate(playerCards[Random.Range(0, playerCards.Count)], Vector3.zero, Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCard.name = "PlayerCard_" + (cardsInHand + i); // Nombrar las cartas según su orden
        }
        dropZoneManager.OnDrawButtonPressed();
    }

    void Update()
    {
        // Verificar si ya hay 4 cartas en el área del jugador y desactivar el botón de la baraja en consecuencia
        deckButton.interactable = PlayerArea.transform.childCount < 4;
    }
}
