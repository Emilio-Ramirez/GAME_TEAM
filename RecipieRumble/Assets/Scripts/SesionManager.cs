using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Registration : MonoBehaviour
{
  public TMP_InputField usernameField; // Use TMP_InputField for TextMeshPro InputFields
  public TMP_InputField passwordField;

  public Button submitButton;

  public void CallRegister(){

    StartCoroutine(Register());

  }

  public void callLogin(){
    StartCoroutine(Login());
  }

 
   IEnumerator Register()
  {
      // Create a new instance of the WWWForm class
      WWWForm form = new WWWForm();

      // Add the username and password fields to the form
      form.AddField("username", usernameField.text);
      form.AddField("password", passwordField.text);

      // Create the URL for the registration API endpoint
      string url = "http://localhost:3000/register";

      // Send the POST request to the server
      using (UnityWebRequest www = UnityWebRequest.Post(url, form))
      {
          yield return www.SendWebRequest();

          if (www.result == UnityWebRequest.Result.Success)
          {
              // Registration successful
              Debug.Log("User registered successfully");
          }
          else
          {
              // Registration failed
              Debug.LogError("Error registering user: " + www.error);
          }
      }
  }

   IEnumerator Login()
    {
        // Create a new instance of the WWWForm class
        WWWForm form = new WWWForm();

        // Add the username and password fields to the form
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        // Create the URL for the login API endpoint
        string url = "http://localhost:3000/login";

        // Send the POST request to the server
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Login successful
                Debug.Log("User logged in successfully");

                // Save the user's authentication token
                string token = www.downloadHandler.text;
                PlayerPrefs.SetString("token", token);

                

                // Perform any additional actions after successful login
                // For example, load the next scene or update the UI
            }
            else
            {
                // Login failed
                Debug.LogError("Error logging in: " + www.error);

                // Display an error message to the user or handle the error appropriately
            }
        }
    }

   public void VerifyInputs()
  {
      // Enable the submit button if the username and password fields are not empty
      submitButton.interactable = (usernameField.text.Length >= 8 && passwordField.text.Length >= 8);
  }




}


