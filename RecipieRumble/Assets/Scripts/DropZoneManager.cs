using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneManager : MonoBehaviour
{
    public GameObject dropZonePrefab;
    public GameObject[] dropZones;
    public int numberOfDropZones = 4;
    public int[] turnsLeft;
    public int[] initialTurns;  // Almacenar los turnos iniciales
    public List<GameObject>[] cardsInDropZone;

    public Transform playerArea; // Área del jugador

    void Start()
    {
        dropZones = new GameObject[numberOfDropZones];
        turnsLeft = new int[numberOfDropZones];
        initialTurns = new int[numberOfDropZones];  // Inicializar el array de turnos iniciales
        cardsInDropZone = new List<GameObject>[numberOfDropZones];
        Vector3 centerPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < numberOfDropZones; i++)
        {
            dropZones[i] = Instantiate(dropZonePrefab, transform);
            dropZones[i].transform.position = new Vector3(centerPosition.x, centerPosition.y + (i * 2.0f - numberOfDropZones + 1) * 50, centerPosition.z);
            turnsLeft[i] = i + 1;
            initialTurns[i] = turnsLeft[i];  // Guardar los turnos iniciales
            cardsInDropZone[i] = new List<GameObject>();

            Collider2D collider = dropZones[i].AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            Debug.Log($"DropZone {i} created with {turnsLeft[i]} turns left.");
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
                    foreach (GameObject card in cardsInDropZone[i])
                    {
                        Destroy(card);
                    }
                    cardsInDropZone[i].Clear();
                    turnsLeft[i] = initialTurns[i];  // Resetear a los turnos iniciales específicos
                    Debug.Log($"Cards discarded and turns reset in Drop Zone {i}");
                }
            }
        }
    }

    public void OnCardDropped(int dropZoneIndex, GameObject card)
    {
        Debug.Log($"Card dropped in Drop Zone {dropZoneIndex}");
        if (cardsInDropZone[dropZoneIndex].Count == 0) // Asegurarse de que no haya cartas ya en la zona
        {
            cardsInDropZone[dropZoneIndex].Add(card);
            Debug.Log($"Card added to Drop Zone {dropZoneIndex}, new count: {cardsInDropZone[dropZoneIndex].Count}");
        }
        else
        {
            Debug.Log($"Drop Zone {dropZoneIndex} already has a card. Cannot add another.");
        }
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
    }

    public void ReturnCardToDropZone(int dropZoneIndex, GameObject card)
    {
        Debug.Log($"Returning card to Drop Zone {dropZoneIndex}");
        if (cardsInDropZone[dropZoneIndex].Count == 0) // Asegurarse de que no haya cartas ya en la zona
        {
            card.transform.SetParent(dropZones[dropZoneIndex].transform, false);
            cardsInDropZone[dropZoneIndex].Add(card);
            Debug.Log($"Card returned to Drop Zone {dropZoneIndex}, new count: {cardsInDropZone[dropZoneIndex].Count}");
        }
        else
        {
            Debug.Log($"Drop Zone {dropZoneIndex} already has a card. Cannot add another.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject card = other.gameObject;
        Debug.Log($"Card detected in Drop Zone area: {card.name}");
    }
}
