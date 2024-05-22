using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class SesionManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TMP_Text logedInText;
    public Button submitButton;
    public Button enterGameButton;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        Debug.Log("Token: " + PlayerPrefs.GetString("token"));
        Debug.Log("Username: " + PlayerPrefs.GetString("username"));
        
        if (PlayerPrefs.GetString("token") != "")
        {
            // User is already logged in

            logedInText.text = "Hello " + PlayerPrefs.GetString("username");
            enterGameButton.interactable = true;
        }
        else
        {
            // User is not logged in
            logedInText.text = "Please log in to enter the game.";
            enterGameButton.interactable = false;
        }
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        string url = "http://localhost:3000/register";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User registered successfully");
                SceneChanger.GoToHome();
            }
            else
            {
                Debug.LogError("Error registering user: " + www.error);
            }
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        string url = "http://localhost:3000/login";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User logged in successfully");
                string token = www.downloadHandler.text;
                PlayerPrefs.SetString("token", token);
                PlayerPrefs.SetString("username", usernameField.text);
                SceneChanger.GoToHome();
            }
            else
            {
                Debug.LogError("Error logging in: " + www.error);
                string errorMessage = "Login failed. Please try again.";
                // Display an error message to the user or handle the error appropriately
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (usernameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
