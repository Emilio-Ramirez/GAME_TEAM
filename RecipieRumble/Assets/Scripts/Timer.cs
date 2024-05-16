using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float totalTime = 300f;  // Total time for the game in seconds (5 minutes)
    private float currentTime;  // Current time remaining
    private bool isTimerRunning = false;
    public delegate void TimeUpAction();
    public static event TimeUpAction OnTimeUp;  // Event triggered when time is up

    public TMP_Text timerText;  

    public Button startButton;  // Reference to the UI button to start the timer

    void Start()
    {
        // Ensure the timer is not running at the start
        isTimerRunning = false;
        
        // Optional: Initialize the timer text
        UpdateTimerText();

        // Assign the StartTimer method to the button's onClick event
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartTimer);
        }
    }

    void Update()
    {
        // Update the timer if it's running
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;  // Reduce the current time by deltaTime each frame
            if (currentTime <= 0)
            {
                // Timer has run out
                currentTime = 0;
                isTimerRunning = false;
                Debug.Log("Time is up!");
                if (OnTimeUp != null)
                {
                    OnTimeUp();  // Trigger the event when time is up
                }
            }

            UpdateTimerText();  // Update the UI text component with the current time
        }
    }

    void UpdateTimerText()
    {
        // Update the UI text component with the current time
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        // Start the timer
        isTimerRunning = true;
        currentTime = totalTime;
        
        // Hide the start button
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void StopTimer()
    {
        // Stop the timer
        isTimerRunning = false;
    }

    public float GetCurrentTime()
    {
        // Get the current time remaining
        return currentTime;
    }

    public bool IsTimerRunning()
    {
        // Check if the timer is running
        return isTimerRunning;
    }
}