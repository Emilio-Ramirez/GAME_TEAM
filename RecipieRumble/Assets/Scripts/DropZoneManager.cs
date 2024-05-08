using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneManager : MonoBehaviour
{
    public GameObject dropZonePrefab; // Assign the drop zone prefab in the inspector
    public GameObject[] dropZones; // This will store the drop zones
    public int numberOfDropZones = 4; // Set the number of drop zones needed
    public int[] turnsLeft; // This will track how many turns each drop zone has left

    void Start()
    {
        // Initialize and instantiate drop zones
        dropZones = new GameObject[numberOfDropZones];
        turnsLeft = new int[numberOfDropZones];
        Vector3 centerPosition = new Vector3(0, 0, 0); // Center of the scene

        for (int i = 0; i < numberOfDropZones; i++)
        {
            dropZones[i] = Instantiate(dropZonePrefab, transform);
            dropZones[i].transform.position = new Vector3(centerPosition.x, centerPosition.y + (i * 2.0f - numberOfDropZones + 1) * 50, centerPosition.z); // Modify '50' to adjust spacing
            turnsLeft[i] = i; // Set different lifespan for each drop zone as required

            // Add a collider to each drop zone
            Collider2D collider = dropZones[i].AddComponent<BoxCollider2D>();
            collider.isTrigger = true; // Set collider as trigger
        }
    }

    // Call this method when the draw button is pressed
    public void OnDrawButtonPressed()
    {
        for (int i = 0; i < turnsLeft.Length; i++)
        {
            if (turnsLeft[i] > 0)
            {
                turnsLeft[i]--;
            }
            else
            {
                // Deactivate drop zone when its time expires
                dropZones[i].SetActive(false);
            }
        }
    }

    // Called when a card is dropped onto a drop zone
    public void OnCardDropped(int dropZoneIndex)
    {
        Debug.Log("Card dropped onto drop zone " + dropZoneIndex);
        // Add your logic here for handling the dropped card
    }
}
