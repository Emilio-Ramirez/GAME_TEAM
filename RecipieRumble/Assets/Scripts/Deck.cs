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
    public GameManager gameManager;

    private bool cardMoved = false; // Indicar si una carta ha sido movida

    List<GameObject> playerCards = new List<GameObject>(); // Lista para las cartas del jugador
    List<GameObject> recipeCardPrefabs = new List<GameObject>(); // Lista para los prefabs de cartas de receta
    List<GameObject> instantiatedRecipeCards = new List<GameObject>(); // Lista para las cartas de receta instanciadas

    void Start()
    {
        if (!Card1 || !Card2 || !RecipeArea || !PlayerArea || !dropZoneManager || !deckButton || !gameManager)
        {
            Debug.LogError("Uno o más componentes esenciales no están asignados en el script Deck.");
            return; // Detener la ejecución si faltan componentes críticos
        }

        // Inicializar las listas de cartas del jugador
        playerCards.Add(Card2); // Solo agregar Card2 para el jugador
        
        // Inicializar las listas de cartas de receta
        recipeCardPrefabs.Add(Card1); // Solo agregar Card1 para la receta

        InitializeRecipeCards(); // Configurar las cartas de receta solo una vez

        // Agregar el listener al botón de la baraja
        deckButton.onClick.AddListener(OnClick);
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
        Debug.Log("Deck button clicked.");

        if (cardMoved)
        {
            Debug.Log("Registering turn because a card was moved.");
            // Registrar un cambio de turno solo si se ha movido una carta
            dropZoneManager.RegisterTurn();
            cardMoved = false; // Reiniciar el indicador después de registrar el turno
        }

        gameManager.RefreshDeck();

        // Actualizar el conteo de cartas en el DropZoneManager después de agregar nuevas cartas
        dropZoneManager.UpdateCardCount();
    }

    void Update()
    {
        int playerCardCount = PlayerArea.transform.childCount;
        //  Debug.Log($"Player card count: {playerCardCount}");
        deckButton.interactable = playerCardCount < 4;

        // Verificar si se ha movido una carta y actualizar el botón de la baraja
        if (cardMoved)
        {
            //Debug.Log("Card was moved, enabling deck button.");
            deckButton.interactable = true;
        }
    }

    // Método para ser llamado cuando se mueve una carta entre DropZones
    public void CardMoved()
    {
        Debug.Log("Card moved between drop zones.");
        cardMoved = true;
    }
}