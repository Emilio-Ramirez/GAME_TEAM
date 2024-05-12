using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public Transform RecipeArea; // Ensure this is assigned in the Unity Editor
    public GameObject PlayerArea; // Ensure this is assigned in the Unity Editor
    public DropZoneManager dropZoneManager; // Ensure this is assigned in the Unity Editor

    List<GameObject> playerCards = new List<GameObject>(); // List for player cards
    List<GameObject> recipeCardPrefabs = new List<GameObject>(); // List for recipe card prefabs
    List<GameObject> instantiatedRecipeCards = new List<GameObject>(); // List for instantiated recipe cards

    void Start()
    {
        if (!Card1 || !Card2 || !Card3 || !RecipeArea || !PlayerArea || !dropZoneManager)
        {
            Debug.LogError("One or more essential components are not assigned in the Deck script.");
            return; // Stop further execution if critical components are missing
        }

        // Initialize the card lists
        playerCards.Add(Card1);
        playerCards.Add(Card2);
        
        // Assuming these are the prefabs you might want to use exclusively for recipes:
        recipeCardPrefabs.Add(Card3);
        recipeCardPrefabs.Add(Card3);

        InitializeRecipeCards(); // Setup recipe cards only once
    }

    void InitializeRecipeCards()
    {
        for (int i = 0; i < 4; i++) // Assuming you want exactly 4 recipe cards
        {
            GameObject recipeCard = Instantiate(recipeCardPrefabs[Random.Range(0, recipeCardPrefabs.Count)], Vector3.zero, Quaternion.identity);
            recipeCard.transform.SetParent(RecipeArea, false);
            recipeCard.name = "RecipeCard_" + i;
            instantiatedRecipeCards.Add(recipeCard); // Keep track of the instantiated recipe cards
        }
    }

    public void OnClick()
    {
        int cardsInHand = PlayerArea.transform.childCount;
        int cardsNeeded = 4 - cardsInHand; // Calculate how many cards are needed to reach 4

        for (int i = 0; i < cardsNeeded; i++)
        {
            GameObject playerCard = Instantiate(playerCards[Random.Range(0, playerCards.Count)], Vector3.zero, Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCard.name = "PlayerCard_" + (cardsInHand + i); // Name the cards based on their order
        }
        dropZoneManager.OnDrawButtonPressed();
    }
}
