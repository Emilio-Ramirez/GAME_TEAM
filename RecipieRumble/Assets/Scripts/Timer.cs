// using UnityEngine;
// using UnityEngine.UI;

// public class Timer : MonoBehaviour
// {
//     public float totalTime = 300f;  // Total time for the game in seconds (5 minutes)
//     private float currentTime;  // Current time remaining
//     private bool isTimerRunning = false;
//     public delegate void TimeUpAction();
//     public static event TimeUpAction OnTimeUp;  // Event triggered when time is up

//     public Text timerText;  // Reference to the UI text component to display the timer

//     void Start()
//     {
//         // Start the timer when the game starts
//         StartTimer();
//     }

//     void Update()
//     {
//         // Update the timer if it's running
//         if (isTimerRunning)
//         {
//             currentTime -= Time.deltaTime;  // Reduce the current time by deltaTime each frame
//             if (currentTime <= 0)
//             {
//                 // Timer has run out
//                 currentTime = 0;
//                 isTimerRunning = false;
//                 Debug.Log("Time is up!");
//                 if (OnTimeUp != null)
//                 {
//                     OnTimeUp();  // Trigger the event when time is up
//                 }
//             }

//             UpdateTimerText();  // Update the UI text component with the current time
//         }
//     }

//     void UpdateTimerText()
//     {
//         // Update the UI text component with the current time
//         if (timerText != null)
//         {
//             int minutes = Mathf.FloorToInt(currentTime / 60);
//             int seconds = Mathf.FloorToInt(currentTime % 60);
//             timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
//         }
//     }

//     public void StartTimer()
//     {
//         // Start the timer
//         isTimerRunning = true;
//         currentTime = totalTime;
//     }

//     public void StopTimer()
//     {
//         // Stop the timer
//         isTimerRunning = false;
//     }

//     public float GetCurrentTime()
//     {
//         // Get the current time remaining
//         return currentTime;
//     }

//     public bool IsTimerRunning()
//     {
//         // Check if the timer is running
//         return isTimerRunning;
//     }
// }
