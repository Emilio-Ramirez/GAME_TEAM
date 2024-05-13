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

    public void ReturnCardToPlayerArea(GameObject card, Transform playerArea)
    {
        if (card == null || playerArea == null) {
            Debug.LogError("Invalid card or player area.");
            return;
        }

        // Mover la carta de regreso a la zona del jugador
        card.transform.SetParent(playerArea, false);
        card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // O ajustar según el layout del área del jugador

        Debug.Log("Card returned to player area.");
    }
}
