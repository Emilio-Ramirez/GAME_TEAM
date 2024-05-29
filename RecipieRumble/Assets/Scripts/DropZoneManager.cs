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

    void Awake()
    {
        Debug.Log("DropZoneManager Awake");
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
        Debug.Log("DropZoneManager Start");
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
            Debug.Log($"DropZone {i} created with {turnsLeft[i]} turns left.");
        }

        if (startButton != null)
        {
            Debug.Log("Start Button is assigned");
            startButton.onClick.AddListener(OnStartButtonPressed);
        }
        else
        {
            Debug.LogError("Start Button is not assigned");
        }

        if (drawButton != null)
        {
            Debug.Log("Draw Button is assigned");
            drawButton.onClick.AddListener(OnDrawButtonPressed);
        }
        else
        {
            Debug.LogError("Draw Button is not assigned");
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
        Debug.Log("Start Button Pressed");

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
        Debug.Log("Draw Button Pressed");
        for (int i = 0; i < numberOfDropZones; i++)
        {
            Debug.Log($"Checking Drop Zone {i} - Turns Left: {turnsLeft[i]}, Card Count: {cardsInDropZone[i].Count}");
            if (cardsInDropZone[i].Count > 0)
            {
                turnsLeft[i]--;
                Debug.Log($"Turns decremented in Drop Zone {i}. New Turns Left: {turnsLeft[i]}");
                if (turnsLeft[i] <= 0)
                {
                    Debug.Log($"Discarding cards in Drop Zone {i}");
                    foreach (GameObject card in cardsInDropZone[i])
                    {
                        Debug.Log($"Destroying card {card.name} in Drop Zone {i}");
                        Destroy(card);
                    }
                    cardsInDropZone[i].Clear();
                    turnsLeft[i] = initialTurns[i];
                    Debug.Log($"Cards discarded and turns reset in Drop Zone {i}");
                }
            }
        }
        UpdateCardCount();
    }

    public void OnCardDropped(int dropZoneIndex, GameObject card)
    {
        Debug.Log($"Card dropped in Drop Zone {dropZoneIndex}");
        if (cardsInDropZone[dropZoneIndex].Count == 0)
        {
            cardsInDropZone[dropZoneIndex].Add(card);
            Debug.Log($"Card added to Drop Zone {dropZoneIndex}, new count: {cardsInDropZone[dropZoneIndex].Count}");
        }
        else
        {
            Debug.Log($"Drop Zone {dropZoneIndex} already has a card. Cannot add another.");
        }

        UpdateCardCount();
    }

    public void RemoveCardFromDropZone(int dropZoneIndex, GameObject card)
    {
        Debug.Log($"Removing card from Drop Zone {dropZoneIndex}");
        if (cardsInDropZone[dropZoneIndex].Remove(card))
        {
            card.transform.SetParent(playerArea, false);
            Debug.Log($"Card removed from Drop Zone {dropZoneIndex} and moved to Player Area");
        }
        else
        {
            Debug.Log($"Card not found in Drop Zone {dropZoneIndex}");
        }

        UpdateCardCount();
    }

    public void UpdateCardCount()
    {
        int cardCount = 0;
        foreach (var cardList in cardsInDropZone)
        {
            cardCount += cardList.Count;
        }
        Debug.Log("Total cards in drop zones: " + cardCount);

        if (cardCount == 4)
        {
            GameManager.Instance.CheckRecipeCombination();
        }
    }
}
