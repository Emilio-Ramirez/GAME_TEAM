using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZoneManager : MonoBehaviour
{
    public static DropZoneManager Instance { get; private set; }

    public GameObject dropZonePrefab;
    public GameObject[] dropZones;
    public int numberOfDropZones = 4;
    public int[] turnsLeft;
    public int[] initialTurns;
    public List<GameObject>[] cardsInDropZone;
    public Transform playerArea;
    public Transform recipeArea;
    public Button startButton;
    public Button drawButton;
    public Deck deck; // Referencia al script Deck

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dropZones = new GameObject[numberOfDropZones];
        turnsLeft = new int[numberOfDropZones];
        initialTurns = new int[numberOfDropZones];
        cardsInDropZone = new List<GameObject>[numberOfDropZones];
        Vector3 centerPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < numberOfDropZones; i++)
        {
            dropZones[i] = Instantiate(dropZonePrefab, transform);
            dropZones[i].transform.position = new Vector3(centerPosition.x, centerPosition.y + (i * 2.0f - numberOfDropZones + 1) * 50, centerPosition.z);
            turnsLeft[i] = i + 1;
            initialTurns[i] = turnsLeft[i];
            cardsInDropZone[i] = new List<GameObject>();

            Collider2D collider = dropZones[i].AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            dropZones[i].SetActive(false);
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
        }

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonPressed);
        }

        if (recipeArea != null)
        {
            recipeArea.gameObject.SetActive(false);
        }

        if (playerArea != null)
        {
            playerArea.gameObject.SetActive(false);
        }
    }

    void OnStartButtonPressed()
    {
        if (recipeArea != null)
        {
            recipeArea.gameObject.SetActive(true);
        }

        if (playerArea != null)
        {
            playerArea.gameObject.SetActive(true);
        }

        for (int i = 0; i < numberOfDropZones; i++)
        {
            dropZones[i].SetActive(true);
        }

        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void OnDrawButtonPressed()
    {
        Debug.Log("Draw button pressed.");

        MoveCardsLeft();

        for (int i = 0; i < numberOfDropZones; i++)
        {
            if (cardsInDropZone[i].Count > 0)
            {
                turnsLeft[i]--;
                if (turnsLeft[i] <= 0)
                {
                    turnsLeft[i] = initialTurns[i];
                }
                Debug.Log($"DropZone {i} turns left: {turnsLeft[i]}");
            }
        }
        UpdateCardCount();
    }

    public void MoveCardsLeft()
    {
        for (int i = 0; i < numberOfDropZones; i++)
        {
            if (cardsInDropZone[i].Count > 0)
            {
                for (int j = cardsInDropZone[i].Count - 1; j >= 0; j--)
                {
                    GameObject card = cardsInDropZone[i][j];
                    if (card == null)
                    {
                        cardsInDropZone[i].RemoveAt(j);
                        continue;
                    }

                    if (i == 0)
                    {
                        Destroy(card);
                        cardsInDropZone[i].RemoveAt(j);
                        Debug.Log($"Card destroyed in DropZone {i}");
                    }
                    else
                    {
                        cardsInDropZone[i - 1].Add(card);
                        card.transform.SetParent(dropZones[i - 1].transform, false);
                        cardsInDropZone[i].RemoveAt(j);
                        Debug.Log($"Card moved from DropZone {i} to DropZone {i - 1}");
                    }
                }
            }
        }
    }

    public void OnCardDropped(int dropZoneIndex, GameObject card)
    {
        cardsInDropZone[dropZoneIndex].Add(card);
        UpdateCardCount();
        Debug.Log($"Card dropped in DropZone {dropZoneIndex}");
    }

    public void MoveCardBetweenDropZones(GameObject card, int fromDropZoneIndex, int toDropZoneIndex)
    {
        Debug.Log($"Moving card from DropZone {fromDropZoneIndex} to DropZone {toDropZoneIndex}");
        if (cardsInDropZone[fromDropZoneIndex].Remove(card))
        {
            cardsInDropZone[toDropZoneIndex].Add(card);
            card.transform.SetParent(dropZones[toDropZoneIndex].transform, false);
            card.transform.SetAsLastSibling();
            ResetTurns(fromDropZoneIndex, toDropZoneIndex);

            // Indicar que una carta ha sido movida
            if (deck != null)
            {
                deck.CardMoved();
            }
            Debug.Log("Card moved successfully.");
        }
        else
        {
            Debug.Log("Failed to move card.");
        }

        UpdateCardCount();
    }

    private void ResetTurns(int fromDropZoneIndex, int toDropZoneIndex)
    {
        turnsLeft[fromDropZoneIndex] = initialTurns[fromDropZoneIndex];
        turnsLeft[toDropZoneIndex] = initialTurns[toDropZoneIndex];
        Debug.Log($"Turns reset: DropZone {fromDropZoneIndex} -> {turnsLeft[fromDropZoneIndex]}, DropZone {toDropZoneIndex} -> {turnsLeft[toDropZoneIndex]}");
    }

    public void UpdateCardCount()
    {
        int cardCount = 0;
        foreach (var cardList in cardsInDropZone)
        {
            cardList.RemoveAll(card => card == null);
            cardCount += cardList.Count;
        }

        if (cardCount == 4)
        {
            GameManager.Instance.CheckRecipeCombination();
        }
        Debug.Log($"Total cards in drop zones: {cardCount}");
    }

    public void RegisterTurn()
{
    Debug.Log("Registering turn.");
    for (int i = 0; i < numberOfDropZones; i++)
    {
        if (cardsInDropZone[i].Count > 0)
        {
            turnsLeft[i]--;
            if (turnsLeft[i] <= 0)
            {
                turnsLeft[i] = initialTurns[i];
            }
            Debug.Log($"DropZone {i} turns left: {turnsLeft[i]}");
        }
    }
    UpdateCardCount();
}

}