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
        for(var i = 0; i < 4; i++){
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);

            GameObject recipieCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            recipieCard.transform.SetParent(RecipieCards.transform, false);
        }
    }
}
