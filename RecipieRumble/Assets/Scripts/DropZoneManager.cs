using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneManager : MonoBehaviour
{
    public GameObject dropZonePrefab; // Assign the drop zone prefab in the inspector
    public GameObject[] dropZones; // Assign these in the inspector
    public int numberOfDropZones = 4; // Set the number of drop zones needed
    public int[] turnsLeft; // This will track how many turns each drop zone has left

    void Start()
    {
         // Initialize and instantiate drop zones
        dropZones = new GameObject[numberOfDropZones];
        turnsLeft = new int[numberOfDropZones];
        for (int i = 0; i < numberOfDropZones; i++) {
            dropZones[i] = Instantiate(dropZonePrefab, transform);
            turnsLeft[i] = i; // Set different lifespan for each drop zone as required
        }
    }

    // Call this method when the draw button is pressed
    public void OnDrawButtonPressed()
    {
        for (int i = 0; i < turnsLeft.Length; i++) {
            if (turnsLeft[i] > 0) {
                turnsLeft[i]--;
            } else {
                // Deactivate drop zone when its time expires
                dropZones[i].SetActive(false);
            }
        }
    }
}

