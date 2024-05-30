// using System.Collections.Generic;
// using System.Linq; // Asegúrate de agregar esta línea
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class PicnicCardConfig : MonoBehaviour
// {
//     public HashSet<int> validCardIdsForRecipeArea = new HashSet<int> { 1, 2, 3, 4 };
//     public HashSet<int> validCardIdsForPlayerArea = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 31, 45, 46 };
//     public GameObject recipeCardPrefab;
//     public GameObject cardPrefab;

//     private GameManager gameManager;

//     private void Start()
//     {
//         gameManager = GameManager.Instance;
//         gameManager.OnCardsFetched += OnCardsFetched;
//     }

//     private void OnDestroy()
//     {
//         if (gameManager != null)
//         {
//             gameManager.OnCardsFetched -= OnCardsFetched;
//         }
//     }

//     private void OnCardsFetched(List<Card> cards)
//     {
//         AddCardsToRecipeArea(cards);
//         ShuffleAndAddNewCardsToPlayerArea(cards);
//     }

//     public void AddCardsToRecipeArea(List<Card> cards)
//     {
//         Transform recipeCardParent = gameManager.recipeCardParent;

//         if (recipeCardParent == null)
//         {
//             Debug.LogError("recipeCardParent is null.");
//             return;
//         }

//         foreach (Transform child in recipeCardParent)
//         {
//             Destroy(child.gameObject);
//         }

//         foreach (Card card in cards)
//         {
//             if (!validCardIdsForRecipeArea.Contains(card.id_carta))
//             {
//                 Debug.LogWarning($"Skipping card with invalid ID for recipe area: {card.id_carta}");
//                 continue;
//             }

//             GameObject newCard = Instantiate(recipeCardPrefab, recipeCardParent);
//             if (newCard == null)
//             {
//                 Debug.LogError("Failed to instantiate new card prefab for recipe area.");
//                 continue;
//             }

//             Image cardImage = newCard.GetComponent<Image>();
//             if (cardImage != null)
//             {
//                 string imagePath = $"Sprites/Picnic/EventDishes/{card.id_carta}";
//                 cardImage.sprite = Resources.Load<Sprite>(imagePath);
//                 if (cardImage.sprite == null)
//                 {
//                     Debug.LogError($"Image not found for card: {imagePath}");
//                 }
//             }
//         }
//     }

//     public void ShuffleAndAddNewCardsToPlayerArea(List<Card> cards)
//     {
//         Transform playerCardParent = gameManager.playerCardParent;

//         if (playerCardParent == null)
//         {
//             Debug.LogError("playerCardParent is null.");
//             return;
//         }

//         int currentCardCount = playerCardParent.childCount;
//         int newCardsCount = 4 - currentCardCount;

//         if (newCardsCount <= 0)
//         {
//             Debug.Log("Player already has 4 cards. No new cards will be added.");
//             return;
//         }

//         List<Card> randomCards = new List<Card>(cards);
//         randomCards.Shuffle();

//         int cardsAdded = 0;
//         HashSet<int> existingCardIds = new HashSet<int>();
//         HashSet<string> existingCardTypes = new HashSet<string>();

//         foreach (Transform child in playerCardParent)
//         {
//             TMP_Text nameText = child.Find("NameText")?.GetComponent<TMP_Text>();
//             TMP_Text typeText = child.Find("TypeText")?.GetComponent<TMP_Text>();
//             if (nameText != null && typeText != null)
//             {
//                 Card cardInHand = cards.FirstOrDefault(c => c.nombre == nameText.text);
//                 if (cardInHand != null)
//                 {
//                     existingCardIds.Add(cardInHand.id_carta);
//                     existingCardTypes.Add(typeText.text);
//                 }
//             }
//         }

//         foreach (Card card in randomCards)
//         {
//             if (cardsAdded >= newCardsCount)
//             {
//                 break;
//             }

//             if (!validCardIdsForPlayerArea.Contains(card.id_carta))
//             {
//                 continue;
//             }

//             if (existingCardIds.Contains(card.id_carta) || existingCardTypes.Contains(card.tipo))
//             {
//                 continue;
//             }

//             GameObject newCard = Instantiate(cardPrefab, playerCardParent);
//             if (newCard == null)
//             {
//                 Debug.LogError("Failed to instantiate new card prefab for player area.");
//                 continue;
//             }

//             TMP_Text nameText = newCard.transform.Find("NameText")?.GetComponent<TMP_Text>();
//             TMP_Text valueText = newCard.transform.Find("ValueText")?.GetComponent<TMP_Text>();
//             TMP_Text typeText = newCard.transform.Find("TypeText")?.GetComponent<TMP_Text>();
//             Image cardImage = newCard.transform.Find("Image")?.GetComponent<Image>();

//             if (nameText != null && valueText != null && typeText != null)
//             {
//                 nameText.text = card.nombre;
//                 valueText.text = card.valor_nutrimental.ToString();
//                 typeText.text = card.tipo;
//                 newCard.name = card.nombre;
//             }

//             if (cardImage != null)
//             {
//                 string imagePath = $"Sprites/Picnic/{card.id_carta}";
//                 cardImage.sprite = Resources.Load<Sprite>(imagePath);
//                 if (cardImage.sprite == null)
//                 {
//                     Debug.LogError($"Image not found for card: {imagePath}");
//                 }
//             }

//             existingCardIds.Add(card.id_carta);
//             existingCardTypes.Add(card.tipo);
//             cardsAdded++;
//         }

//         gameManager.UpdateScoreUI();
//         gameManager.dropZoneManager?.UpdateCardCount();
//     }
// }
