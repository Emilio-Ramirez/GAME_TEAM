using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject RecipieCards;
    public GameObject PlayerArea; 
     public DropZoneManager dropZoneManager; 

    List<GameObject> cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    // New version of the card
    public void OnClick()
    {
       int cardsInHand = PlayerArea.transform.childCount;
        int cardsNeeded = 4 - cardsInHand; // Calculate how many cards are needed to reach 4

        for (int i = 0; i < cardsNeeded; i++) { // Only instantiate the number of cards needed
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCard.name = "PlayerCard_" + (cardsInHand + i); // Name the cards based on their order

            GameObject recipieCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            recipieCard.transform.SetParent(RecipieCards.transform, false);
            recipieCard.name = "RecipeCard_" + (cardsInHand + i);
        }
        dropZoneManager.OnDrawButtonPressed();
    }
}
