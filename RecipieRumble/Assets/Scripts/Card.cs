using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card 
{

   public int cardId;
   public string cardName;
   public string cardValue;
   public int cardType; 
}

[System.Serializable]
public class Cards
{
   public List<Card> cards;
}